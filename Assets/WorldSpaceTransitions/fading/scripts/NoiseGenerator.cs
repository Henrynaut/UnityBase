//Script to create seamless noise textures
/*some of the code is based on this CPOL (Code Project Open License (CPOL) 1.02) licenced article by andrea contoli
 * https://www.codeproject.com/Articles/838511/Procedural-seamless-noise-texture-generator
*/
using UnityEngine;

public enum NoiseType { quad = 0, circ = 1, pixel = 2 };
public enum TextureSpace { texture2D = 0, texture3D_Atlas = 1 };

public static class NoiseGenerator {

    public static NoiseType noiseType = NoiseType.quad;

    public static Texture2D Generate2D(float boxSize, int deepness, int delta, int resolution2d)
    {
        int boxPxSize = Mathf.RoundToInt(boxSize * resolution2d);
        float[,] Values2D = new float[resolution2d, resolution2d];

        float mins = 1f;
        float maxs = 0f;

        int iterations = 0;

        for (int i = 0; i < resolution2d; i++)
        {
            for (int j = 0; j < resolution2d; j++)
            {
                Values2D[i, j] = 0;
            }
        }
        int x;
        int y;
        switch ((int)noiseType)
        {
            case 0:
                x = Mathf.RoundToInt(Random.value * (resolution2d - 0.5f));
                y = Mathf.RoundToInt(Random.value * (resolution2d - 0.5f));
                PrintQuad(ref iterations, ref Values2D, resolution2d, resolution2d, x, y, boxPxSize, deepness, delta);
                break;
            case 1:
                x = Mathf.RoundToInt(Random.value * (resolution2d - 0.5f));
                y = Mathf.RoundToInt(Random.value * (resolution2d - 0.5f));
                PrintCirc(ref iterations, ref Values2D, resolution2d, resolution2d, x, y, boxPxSize, deepness, delta);
                break;
            default:break;
        }
        Debug.Log("iterations: " + iterations.ToString());
        Texture2D t = new Texture2D(resolution2d, resolution2d, TextureFormat.RGB24, false);
        Color[] cols = new Color[resolution2d * resolution2d];

        int idx = 0;
        for (int i = 0; i < resolution2d; i++)
        {
            for (int j = 0; j < resolution2d; j++, idx++)
            {
                float nc = Values2D[i, j];
                nc = nc % 512;
                if (nc > 256) nc = 512 - nc;

                float cc = nc / 256f;
                if (noiseType == NoiseType.pixel) cc = Random.value;
                cols[idx] = new Color(cc, cc, cc, 1);
                //mins = Mathf.Min(mins, cc);
                //Debug.Log(mins.ToString()+" ||| "+cc.ToString());
                //maxs = Mathf.Max(maxs, cc);
            }
        }
        t.SetPixels(cols);
        t.Apply();
        return t;
    }


    public static Texture2D Generate3DAtlas(float boxSize, int deepness, int delta, int resolution3d)
    {
        if (resolution3d > 256)
        {
            Debug.LogWarning("max resolution is 256");
            return null;
        }
        int boxPxSize = Mathf.RoundToInt(boxSize * resolution3d);
        float[,,] Values3D = new float[resolution3d, resolution3d, resolution3d];

        for (int i = 0; i < resolution3d; i++)
        {
            for (int j = 0; j < resolution3d; j++)
            {
                for (int k = 0; k < resolution3d; k++)
                {
                    Values3D[i, j, k] = 0;
                }
            }
        }
        int iterations = 0;

        int x;
        int y;
        int z;
        switch ((int)noiseType)
        {
            case 0:
                x = Mathf.RoundToInt(Random.value * (resolution3d - 0.5f));
                y = Mathf.RoundToInt(Random.value * (resolution3d - 0.5f));
                z = Mathf.RoundToInt(Random.value * (resolution3d - 0.5f));
                PrintQuad3D(ref iterations, ref Values3D, resolution3d, resolution3d, resolution3d, x, y, z, boxPxSize, deepness, delta);
                break;
            case 1:
                x = Mathf.RoundToInt(Random.value * (resolution3d - 0.5f));
                y = Mathf.RoundToInt(Random.value * (resolution3d - 0.5f));
                z = Mathf.RoundToInt(Random.value * (resolution3d - 0.5f));
                PrintCirc3D(ref iterations, ref Values3D, resolution3d, resolution3d, resolution3d, x, y, z, boxPxSize, deepness, delta);
                break;
            default: break;
        }
        Debug.Log("iterations: " + iterations.ToString());
        int atlasSize = (int)Mathf.Sqrt(resolution3d);
        int atlasRes = resolution3d * atlasSize;
        Debug.Log("atlasRes " + atlasRes.ToString());
        Texture2D t = new Texture2D(atlasRes, atlasRes, TextureFormat.RGB24, false);


        for (int k = 0; k < resolution3d; k++)
        {
            int idx = 0;
            Color[] cols = new Color[resolution3d * resolution3d];
            for (int j = 0; j < resolution3d; j++)
            {
                for (int i = 0; i < resolution3d; i++)
                {
                    float nc = Values3D[i, j, k];
                    nc = nc % 512;
                    if (nc > 256) nc = 512 - nc;

                    float cc = nc / 256f;
                    if (noiseType == NoiseType.pixel) cc = Random.value;
                    cols[idx] = new Color(cc, cc, cc, 1);
                    idx++;
                }
            }
            int x1 = resolution3d * (k % atlasSize);
            int y1 = atlasSize * (k - (k % atlasSize));
            Debug.Log("x1 " + x1.ToString() + " y1 " + y1.ToString());
            t.SetPixels(x1, y1, resolution3d, resolution3d, cols, 0);
        }

        t.Apply();
        return t;

    }

    #region CONTOLI NOISE //slightly modified scripts from https://www.codeproject.com/Articles/838511/Procedural-seamless-noise-texture-generator
    private static void PrintQuad(ref int iter, ref float[,] Values2D, int wid, int hei, int x, int y, int boxPxSize, int deepness, int delta)
    {
        //iter = 0;

        if (boxPxSize > wid)
            boxPxSize = wid;
        if (boxPxSize > hei)
            boxPxSize = wid;

        if (boxPxSize >= 1)
        {
            iter++;
            for (int i = -boxPxSize / 2; i < boxPxSize / 2; i++)
            {
                for (int j = -boxPxSize / 2; j < boxPxSize / 2; j++)
                {
                    /*seamless management start*/
                    int pixX = (x + i) % wid;
                    int pixY = (y + j) % hei;
                    if (pixX < 0)
                        pixX = wid + pixX;
                    if (pixY < 0)
                        pixY = wid + pixY;
                    /*seamless management end*/

                    Values2D[pixX, pixY] = (Values2D[pixX, pixY] + delta);// % 256; // add value
                                                                             /*                  if(Values[pixX, pixY] + delta < 256)
                                                                                                 {
                                                                                                     Values[pixX, pixY] = Values[pixX, pixY] + delta;
                                                                                                 }
                                                                                                 else
                                                                                                 {
                                                                                                     Values[pixX, pixY] = Values[pixX, pixY] - delta;
                                                                                                 }
                                                                             */
                }
                if (deepness > 1)
                {
                    int xx = Mathf.RoundToInt(Random.Range(x - (int)(boxPxSize * 0.5), x + (int)(boxPxSize * 0.5)));
                    int yy = Mathf.RoundToInt(Random.Range(y - (int)(boxPxSize * 0.5), y + (int)(boxPxSize * 0.5)));

                    PrintQuad(ref iter, ref Values2D, wid, hei, xx, yy, boxPxSize / 2, --deepness, delta);
                }
            }
        }

    }

    private static void PrintQuad3D(ref int iter, ref float[,,] Values, int wid, int hei, int dep, int x, int y, int z, int boxPxSize, int deepness, int delta)
    {

        if (boxPxSize > wid)
            boxPxSize = wid;
        if (boxPxSize > hei)
            boxPxSize = wid;
        //if (boxPxSize > dep)
        //    dep = wid;


        if (boxPxSize >= 1)
        {
            iter++;
            for (int i = -boxPxSize / 2; i < boxPxSize / 2; i++)
            {
                for (int j = -boxPxSize / 2; j < boxPxSize / 2; j++)
                {
                    for (int k = -boxPxSize / 2; k < boxPxSize / 2; k++)
                    {

                        /*seamless management start*/
                        int pixX = (x + i) % wid;
                        int pixY = (y + j) % hei;
                        int pixZ = (z + k) % dep;
                        if (pixX < 0)
                            pixX = wid + pixX;
                        if (pixY < 0)
                            pixY = hei + pixY;
                        if (pixZ < 0)
                            pixZ = dep + pixZ;
                        /*seamless management end*/

                        Values[pixX, pixY, pixZ] = (Values[pixX, pixY, pixZ] + delta); // add value

                        //Iterations++;
                    }
                }
                if (deepness > 1)
                { 
                    int xx = Mathf.RoundToInt(Random.Range(x - (int)(boxPxSize * 0.5), x + (int)(boxPxSize * 0.5)));
                    int yy = Mathf.RoundToInt(Random.Range(y - (int)(boxPxSize * 0.5), y + (int)(boxPxSize * 0.5)));
                    int zz = Mathf.RoundToInt(Random.Range(z - (int)(boxPxSize * 0.5), z + (int)(boxPxSize * 0.5)));

                    PrintQuad3D(ref iter, ref Values, wid, hei, dep, xx, yy, zz, boxPxSize / 2, --deepness, delta);
                }
            }
        }
    }

    private static void PrintCirc(ref int iter, ref float[,] Values, int wid, int hei, int x, int y, int boxSize, int deepness, int delta)
    {

        if (boxSize > wid)
            boxSize = wid;
        if (boxSize > hei)
            boxSize = wid;

        if (boxSize >= 1)
        {
            iter++;
            for (int i = -boxSize / 2; i < boxSize / 2; i++)
            {
                for (int j = -boxSize / 2; j < boxSize / 2; j++)
                {
                    //if ( Math.Pow(i - boxSize / 2,2)  + Math.Pow(j - boxSize / 2 ,2) <= Math.Pow(boxSize / 2,2) 
                    if (Mathf.Pow(i, 2) + Mathf.Pow(j, 2) <= Mathf.Pow(boxSize / 2, 2)
                        )
                    {

                        int pixX = (x + i) % wid; // % --> seamless
                        int pixY = (y + j) % hei; //% --> seamless
                        if (pixX < 0)
                            pixX = wid + pixX;
                        if (pixY < 0)
                            pixY = hei + pixY;

                        Values[pixX, pixY] = Values[pixX, pixY] + delta;

                        
                    }

                }
                if (deepness > 1)
                {
                    int xx = Mathf.RoundToInt(Random.Range(x - (int)(boxSize * 0.5), x + (int)(boxSize * 0.5)));
                    int yy = Mathf.RoundToInt(Random.Range(y - (int)(boxSize * 0.5), y + (int)(boxSize * 0.5)));

                    PrintCirc(ref iter, ref Values, wid, hei, xx, yy, boxSize / 2, --deepness, delta);
                }

            }
        }

    }

    private static void PrintCirc3D(ref int iter, ref float[,,] Values, int wid, int hei, int zlvl, int x, int y, int z, int boxSize, int deepness, int delta)
    {

        if (boxSize > wid)
            boxSize = wid;
        if (boxSize > hei)
            boxSize = wid;
        //if (boxSize > zlvl)
        //    zlvl = wid;


        if (boxSize >= 1)
        {
            iter++;
            for (int i = -boxSize / 2; i < boxSize / 2; i++)
            {
                for (int j = -boxSize / 2; j < boxSize / 2; j++)
                {
                    for (int k = -boxSize / 2; k < boxSize / 2; k++)
                    {

                        if (Mathf.Pow(i, 2) + Mathf.Pow(j, 2) + Mathf.Pow(k, 2) <= Mathf.Pow(boxSize / 2, 2))
                        {

                            /*seamless management start*/
                            int pixX = (x + i) % wid;
                            int pixY = (y + j) % hei;
                            int pixZ = (z + k) % zlvl;
                            if (pixX < 0)
                                pixX = wid + pixX;
                            if (pixY < 0)
                                pixY = wid + pixY;
                            if (pixZ < 0)
                                pixZ = zlvl + pixZ;
                            /*seamless management end*/

                            Values[pixX, pixY, pixZ] = Values[pixX, pixY, pixZ] + delta; // add value

                            
                        }
                    }
                }
                if (deepness > 1)
                {
                    int xx = Mathf.RoundToInt(Random.Range(x - (int)(boxSize * 0.5), x + (int)(boxSize * 0.5)));
                    int yy = Mathf.RoundToInt(Random.Range(y - (int)(boxSize * 0.5), y + (int)(boxSize * 0.5)));
                    int zz = Mathf.RoundToInt(Random.Range(z - (int)(boxSize * 0.5), z + (int)(boxSize * 0.5)));

                    PrintCirc3D(ref iter, ref Values, wid, hei, zlvl, xx, yy, zz, boxSize / 2, --deepness, delta);
                }
            }
        }

    }
    #endregion
}
