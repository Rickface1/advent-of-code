
namespace advent.resources;

/// <summary>
/// static class for calculating different areas,
/// using a variety of theorems
/// </summary>
public static class Area{
    /// <summary>
    /// Returns the inside length of a polygon
    /// </summary>
    /// <param name="vectors"></param>
    /// <returns></returns>
    public static long Inside(List<Vector> vectors) => Shoelace(vectors) - (Vector.GetBorderLength(vectors) / 2) + 1;
    /// <summary>
    /// Returns the inside length of a polygon
    /// </summary>
    /// <param name="indices"></param>
    /// <returns></returns>
    public static long Inside(List<MapIndex> indices) => Shoelace(indices) - (MapIndex.GetTotalLength(indices) / 2) + 1;
    /// <summary>
    /// Does Shoelace method to calculate the area inside polygon formed by the vectors
    /// Requires vectors to be sorted either clockwise or counter clockwise
    /// /// </summary>
    /// <param name="vectors"></param>
    /// <returns></returns>
    public static long Shoelace(List<Vector> vectors){
        long sum = 0;

        for(int i = 0; i < vectors.Count - 1; i++){
            sum += vectors[i].index.line * vectors[i + 1].index.column - vectors[i].index.column * vectors[i + 1].index.line;
        }

        sum += vectors[^1].index.line * vectors[0].index.column - vectors[^1].index.column * vectors[0].index.line;

        return Math.Abs(sum) / 2;
    }
    /// <summary>
    /// Does Shoelace method to calculate the area inside polygon formed by the indexes
    /// Requires indexes to be sorted either clockwise or counter clockwise
    /// /// </summary>
    /// <param name="indexes"></param>
    /// <returns></returns>
    public static long Shoelace(List<MapIndex> indexes){
        long sum = 0;

        for(int i = 0; i < indexes.Count - 1; i++){
            sum += indexes[i].line * indexes[i + 1].column - indexes[i].column * indexes[i + 1].line;
        }

        sum += indexes[^1].line * indexes[0].column - indexes[^1].column * indexes[0].line;

        return Math.Abs(sum) / 2;
    }
    /// <summary>
    /// *NOT USUALLY MORE EFFICIENT*
    /// Does Shoelace method to calculate the area inside polygon formed by the vectors
    /// Requires vectors to be sorted either clockwise or counter clockwise
    /// Does it Parallel
    /// /// </summary>
    /// <param name="vectors"></param>
    /// <returns></returns>
    public static long ParallelInside(List<Vector> vectors){
        long sum = 0;

        Parallel.For(0, vectors.Count - 1, i => {
            Interlocked.Add(ref sum, vectors[i].index.line * vectors[i + 1].index.column - vectors[i].index.column * vectors[i + 1].index.line);
        });

        sum += vectors[^1].index.line * vectors[0].index.column - vectors[^1].index.column * vectors[0].index.line;

        return Math.Abs(sum) / 2;
    }

    /// <summary>
    /// Calculates the total area inside of the polygon formed by vectors using the Shoelace theorem in conjunction with Pick's theorem
    /// </summary>
    /// <param name="vectors"></param>
    /// <returns></returns>
    public static long TotalArea(List<Vector> vectors) => Shoelace(vectors) + (Vector.GetBorderLength(vectors)/2) + 1;
    /// <summary>
    /// Calculates the total area inside of the polygon formed by vectors using the Shoelace theorem in conjunction with Pick's theorem
    /// </summary>
    /// <param name="vectors"></param>
    /// <returns></returns>
    public static long TotalArea(List<MapIndex> indices) => Shoelace(indices) + (MapIndex.GetTotalLength(indices)/2) + 1;
    /// <summary>
    /// *NOT USUALLY MORE EFFICIENT*
    /// Calculates the total area inside of the polygon formed by vectors using the Shoelace theorem in conjunction with Pick's theorem
    /// Uses Parallelization
    /// </summary>
    /// <param name="vectors"></param>
    /// <returns></returns>
    public static long ParallelTotalArea(List<Vector> vectors) => ParallelInside(vectors) + (Vector.GetBorderLength(vectors)/2) + 1;
}