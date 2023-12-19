
namespace advent.resources;

public class Area{
    public static long Inside(List<Vector> vectors){
        long sum = 0;

        for(int i = 0; i < vectors.Count - 1; i++){
            sum += vectors[i].index.line * vectors[i + 1].index.column - vectors[i].index.column * vectors[i + 1].index.line;
        }

        sum += vectors[^1].index.line * vectors[0].index.column - vectors[0].index.line * vectors[^1].index.column;

        return Math.Abs(sum) / 2;
    }

    public static long TotalArea(List<Vector> vectors) => Inside(vectors) + (Vector.GetBorderLength(vectors)/2) + 1;
}