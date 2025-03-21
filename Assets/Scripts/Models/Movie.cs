using System.Collections.Generic;

/// <summary>
/// Represents a movie with its basic information.
/// </summary>
[System.Serializable]
public class Movie
{
    /// <summary>
    /// Unique identifier for the movie.
    /// </summary>
    public int id;
    /// <summary>
    /// Title of the movie.
    /// </summary>
    public string title;
    /// <summary>
    /// Brief overview of the movie.
    /// </summary>
    public string overview;
    /// <summary>
    /// Path to the movie's poster image.
    /// </summary>
    public string poster_path;
}

/// <summary>
/// Represents a response from a movie search query.
/// </summary>
[System.Serializable]
public class MovieSearchResponse
{
    /// <summary>
    /// List of movies that match the search query.
    /// </summary>
    public List<Movie> results;
}

/// <summary>
/// Represents a movie genre.
/// </summary>
[System.Serializable]
public class Genre
{
    /// <summary>
    /// Unique identifier for the genre.
    /// </summary>
    public int id;
    /// <summary>
    /// Name of the genre.
    /// </summary>
    public string name;
}

/// <summary>
/// Represents a cast member in a movie.
/// </summary>
[System.Serializable]
public class Cast
{
    /// <summary>
    /// Name of the cast member.
    /// </summary>
    public string name;
    /// <summary>
    /// Character played by the cast member.
    /// </summary>
    public string character;
    /// <summary>
    /// Path to the cast member's profile image.
    /// </summary>
    public string profile_path;
}

/// <summary>
/// Represents detailed information about a movie.
/// </summary>
[System.Serializable]
public class MovieDetails
{
    /// <summary>
    /// Title of the movie.
    /// </summary>
    public string title;
    /// <summary>
    /// Brief overview of the movie.
    /// </summary>
    public string overview;
    /// <summary>
    /// Path to the movie's poster image.
    /// </summary>
    public string poster_path;
    /// <summary>
    /// Average user rating for the movie.
    /// </summary>
    public float vote_average;
    /// <summary>
    /// List of genres associated with the movie.
    /// </summary>
    public List<Genre> genres;
    /// <summary>
    /// List of cast members in the movie.
    /// </summary>
    public List<Cast> cast;
}