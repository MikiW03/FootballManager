namespace FootballManager;

/// <summary>   Represents a football formation. </summary>
///
/// <param name="defenders">    The number of defenders in the formation. </param>
/// <param name="midfielders">  The number of midfielders in the formation. </param>
/// <param name="forwards">     The number of forwards in the formation. </param>
public class Formation(int defenders, int midfielders, int forwards)
{
    /// <summary>   Gets the number of defenders in the formation. </summary>
    ///
    /// <value> The number of defenders in the formation. </value>
    public int Defenders { get; } = defenders;
    /// <summary>   Gets the number of midfielders in the formation. </summary>
    ///
    /// <value> The number of midfielders in the formation. </value>
    public int Midfielders { get; } = midfielders;
    /// <summary>   Gets the number of forwards in the formation. </summary>
    ///
    /// <value> The number of forwards in the formation. </value>
    public int Forwards { get; } = forwards;
}