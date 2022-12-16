using MySql.Data.MySqlClient;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using TShockAPI.DB;

namespace TShock_AutoTeam;

[ApiVersion(2, 1)]
public class AutoTeam : TerrariaPlugin
{
	public override string Author => "Naram Qashat (CyberBotX)";

	public override string Description => "Automatically join to a player to their previous team (if any) on connect.";

	public override string Name => "AutoTeam";

	public override Version Version => new("1.0");

	public AutoTeam(Main game) : base(game)
	{
	}

	public override void Initialize()
	{
		ServerApi.Hooks.NetGreetPlayer.Register(this, AutoTeam.OnGreetPlayer);
		GetDataHandlers.PlayerTeam += AutoTeam.HandlePlayerTeam;

		// Ensures that we have a table in TShock's database to store the auto team assignments.
		_ = new SqlTableCreator(TShock.DB, TShock.DB.GetSqlType() == SqlType.Sqlite ? new SqliteQueryCreator() : new MysqlQueryCreator()).EnsureTableStructure(
			new("AutoTeam",
				new("ID", MySqlDbType.Int32)
				{
					Primary = true,
					AutoIncrement = true
				},
				new("Username", MySqlDbType.String),
				new("UUID", MySqlDbType.String),
				new("Team", MySqlDbType.Int32)
			)
		);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			_ = ServerApi.Hooks.NetGreetPlayer.Deregister(this, AutoTeam.OnGreetPlayer);
			GetDataHandlers.PlayerTeam -= AutoTeam.HandlePlayerTeam;
		}

		base.Dispose(disposing);
	}

	record class TeamData(int ID, int Team);

	/// <summary>
	/// Attempts to retrieve the player's previous auto team assignment from the TShock database.
	/// </summary>
	/// <param name="username">The username (from <see cref="TSPlayer.Name" />) of the player.</param>
	/// <param name="UUID">The UUID (from <see cref="TSPlayer.UUID" />) of the player.</param>
	/// <returns>If the assignment was in the database, a <see cref="TeamData" /> containing the database row ID and team ID, otherwise <see langword="null" />.</returns>
	static TeamData? GetTeamFromDB(string username, string UUID)
	{
		try
		{
			using var reader = TShock.DB.QueryReader("SELECT * FROM AutoTeam WHERE Username = @0 AND UUID = @1", username, UUID);
			if (reader.Read())
				return new(reader.Get<int>("ID"), reader.Get<int>("Team"));
		}
		catch (Exception ex)
		{
			TShock.Log.Error($"{ex}");
		}
		return null;
	}

	/// <summary>
	/// Updates or inserts the player's auto team assignment into the TShock database.
	/// </summary>
	/// <param name="username"></param>
	/// <param name="UUID"></param>
	/// <param name="team"></param>
	static void StoreTeamInDB(string username, string UUID, int team)
	{
		try
		{
			var teamData = AutoTeam.GetTeamFromDB(username, UUID);
			_ = teamData is null ?
				TShock.DB.Query("INSERT INTO AutoTeam (Username, UUID, Team) VALUES (@0, @1, @2)", username, UUID, team) :
				TShock.DB.Query("UPDATE AutoTeam SET Team = @0 WHERE ID = @1", team, teamData.ID);
		}
		catch (Exception ex)
		{
			TShock.Log.Error($"{ex}");
		}
	}

	/// <summary>
	/// Handles storing the player's auto team assignment into the database (and broadcasts the message since we indicate we handled the event).
	/// </summary>
	/// <param name="sender">The sender (not used).</param>
	/// <param name="e">The <see cref="GetDataHandlers.PlayerTeamEventArgs" /> for the player.</param>
	static void HandlePlayerTeam(object? sender, GetDataHandlers.PlayerTeamEventArgs e)
	{
		// Get the TSPlayer from the event.
		var player = TShock.Players[e.PlayerId];

		// Ensure that we actually did get a player.
		if (player is not null)
		{
			// Get the team name for the broadcast message (assumed that e.Team is always between 0 and 5 inclusive, but the switch expression stills needs that default case).
			string teamName = e.Team switch
			{
				0 => "is no longer on a party.",
				1 => "has joined the red party.",
				2 => "has joined the green party.",
				3 => "has joined the blue party.",
				4 => "has joined the yellow party.",
				5 => "has joined the pink party.",
				_ => ""
			};

			TShock.Utils.Broadcast($"{player.Name} {teamName}", Main.teamColor[e.Team]);

			AutoTeam.StoreTeamInDB(player.Name, player.UUID, e.Team);

			e.Handled = true;
		}
	}

	/// <summary>
	/// Handles joining the player to their team (if any) when they have finished joining the server.
	/// </summary>
	/// <param name="e">The <see cref="GreetPlayerEventArgs" /> for the player.</param>
	static void OnGreetPlayer(GreetPlayerEventArgs e)
	{
		// Get the TSPlayer from the event.
		var player = TShock.Players[e.Who];

		// Ensure that we actually did get a player.
		if (player is not null)
		{
			// Get the player's previous auto team assignment, if any.
			var teamData = AutoTeam.GetTeamFromDB(player.Name, player.UUID);
			// Only proceed if that player did have a previous auto team assignment and they were actually on a team.
			if (teamData is not null && teamData.Team != 0)
			{
				// Get the team name for the messages (the switch expression needs that default case even though teamData.Team should always be between 1 and 5 inclusive).
				string teamName = teamData.Team switch
				{
					1 => "red",
					2 => "green",
					3 => "blue",
					4 => "yellow",
					5 => "pink",
					_ => ""
				};

				// Set the player's team.
				player.SetTeam(teamData.Team);
				// Send the player the message they have been joined to a team.
				player.SendMessage($"You have been automatically joined to the {teamName} party.", Main.teamColor[teamData.Team]);
				// Broadcast the message that the player was joined to a team.
				TShock.Utils.Broadcast($"{player.Name} has been automatically joined to the {teamName} party.", Main.teamColor[teamData.Team]);
			}

			e.Handled = true;
		}
	}
}
