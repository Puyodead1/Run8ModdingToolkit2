using SharpDX;
using JeremyAnsel.Media.WavefrontObj;
using MoreLinq;
using Run8Tools.InternalClasses;

namespace Run8Tools.Commands
{
    public class Avatar : ICommand
    {
        public int Main(string[] args)
        {
            if (args.Length == 1)
            {
                Console.WriteLine("Not enough arguments");
                return 1;
            }

            string outputFileName, outputFilePath;

            string inputFilePath = args[1];
            if (!File.Exists(inputFilePath))
            {
                Console.WriteLine("File does not exist: " + inputFilePath);
                return 1;
            }

            if (!inputFilePath.EndsWith(".rn8"))
            {
                Console.WriteLine("Not a valid model file");
                return 1;
            }

            if (!inputFilePath.Contains("Avatar") && !args.Contains("--force"))
            {
                Console.WriteLine("This doesn't look like a valid avatar model file.");
                return 1;
            }

            string? inputFileDirectory = Path.GetDirectoryName(inputFilePath);
            if (inputFileDirectory == "")
            {
                inputFileDirectory = Directory.GetCurrentDirectory();
            }



            outputFileName = Path.GetFileNameWithoutExtension(inputFilePath) + ".obj";
            outputFilePath = Path.Join(inputFileDirectory, outputFileName);

            try
            {
                return DoWork(inputFilePath, outputFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Command Failed: " + ex.Message);
                return 1;
            }
        }

        public int DoWork(string InputFilePath, string outputFilePath)
        {
            EW[] VertexBuffer;
            int[] IndexBuffer;
            List<string> Textures = new List<string>();
            List<CW> F = new List<CW>();
            KRL H;

            FileStream fs = File.OpenRead(InputFilePath);
            try
            {
                using (BinaryReader binaryReader  = new BinaryReader(fs))
                {
                    List<EW> list = new List<EW>();

                    // Read vertex buffer
                    int VertexCount = binaryReader.ReadInt32() / 7;
                    Console.WriteLine("VertexCount: " + VertexCount);
                    for (int i = 0; i < VertexCount; i++)
                    {
                        EW item = default(EW);
                        binaryReader.ReadSingle();
                        item.SVPosition.X = binaryReader.ReadSingle() * 63.7f;
                        item.Normal.Y = binaryReader.ReadSingle() / -1.732f;
                        item.SVPosition.Z = binaryReader.ReadSingle() / 16f;
                        item.TexCoord0.X = binaryReader.ReadSingle() / 4.8f;
                        item.Normal.X = binaryReader.ReadSingle() / 10.962f;
                        binaryReader.ReadSingle();
                        item.Normal.Z = binaryReader.ReadSingle() / 11.432f;
                        item.TexCoord0.Y = binaryReader.ReadSingle() / 9.6f;
                        item.SVPosition.Y = -binaryReader.ReadSingle() * 6f;
                        item.BlendIndices0.W = (int)binaryReader.ReadByte();
                        item.BlendWeight0.Z = binaryReader.ReadSingle();
                        item.BlendIndices0.X = (int)binaryReader.ReadByte();
                        item.BlendWeight0.Y = binaryReader.ReadSingle();
                        item.BlendIndices0.Y = (int)binaryReader.ReadByte();
                        item.BlendWeight0.W = binaryReader.ReadSingle();
                        item.BlendIndices0.Z = (int)binaryReader.ReadByte();
                        item.BlendWeight0.X = binaryReader.ReadSingle();
                        list.Add(item);
                    }
                    VertexBuffer = list.ToArray();
                    
                    // Read textures
                    int TextureCount = binaryReader.ReadInt32() + 6;
                    Console.WriteLine("TextureCount: " + TextureCount);

                    for (int j = 0; j < TextureCount; j++)
                    {
                        string a = binaryReader.ReadString().Replace(".dds", "");
                        Console.WriteLine("Texture[" + j + "]: " + a);
                        Textures.Add(a);
                    }

                    // Read index buffer
                    bool IsUShort = binaryReader.ReadBoolean();
                    Console.WriteLine("IsUShort: " + IsUShort.ToString());
                    int IndexBufferSize = binaryReader.ReadInt32();
                    Console.WriteLine("IndexBufferSize: " + IndexBufferSize);
                    if (IsUShort)
                    {
                        IndexBuffer = new int[IndexBufferSize];
                        for(int k = 0; k < IndexBufferSize; k++)
                        {
                            IndexBuffer[k] = (ushort)binaryReader.ReadInt32();
                        }
                    }
                    else
                    {
                        IndexBuffer = new int[IndexBufferSize];
                        for (int l = 0; l < IndexBufferSize; l++)
                        {
                            IndexBuffer[l] = binaryReader.ReadInt32();
                        }
                    }

                    // Read textures
                    int TextureCount2 = binaryReader.ReadInt32() - 9;
                    Console.WriteLine("TextureCount2: " + TextureCount2);
                    if (TextureCount2 == 0)
                    {
                        CW item2 = new CW
                        {
                            ElementCount = IndexBuffer.Length,
                            P = 0,
                            F = 0
                        };
                        F.Add(item2);
                    } else
                    {
                        for (int m = 0; m < TextureCount2; m++)
                        {
                            CW cw = new CW();
                            binaryReader.ReadSingle();
                            int index = binaryReader.ReadInt32();
                            string text = Textures[index] + "_maro.tx8";
                            // LOAD TEXTURE
                            cw.ElementCount = binaryReader.ReadInt32();
                            cw.F = binaryReader.ReadInt32();
                            cw.P = binaryReader.ReadInt32();
                            F.Add(cw);
                            
                        }
                    }

                    // Read skeleton hierarchy
                    List<int> SkeletonHierarchy = new List<int>();
                    int SkeletonHierarchyCount = binaryReader.ReadInt32();
                    Console.WriteLine("SkeletonHierarchyCount: " + SkeletonHierarchyCount);
                    for (int n = 0; n < SkeletonHierarchyCount; n++)
                    {
                        SkeletonHierarchy.Add(binaryReader.ReadInt32());
                    }

                    // Read bone indecies
                    Dictionary<string, int> BoneIndecies = new Dictionary<string, int>();
                    int BoneIndecieCount = binaryReader.ReadInt32();
                    Console.WriteLine("BoneIndecieCount: " + BoneIndecieCount);
                    for (int num4 = 0; num4 < BoneIndecieCount; num4++)
                    {
                        string key = binaryReader.ReadString();
                        int value = binaryReader.ReadInt32();
                        Console.WriteLine("BoneIndicie: K:" + key + "; V: " + value);
                        BoneIndecies.Add(key, value);
                    }

                    // Read bind pose
                    List<Matrix> BindPose = new List<Matrix>();
                    int BindPoseCount = binaryReader.ReadInt32();
                    Console.WriteLine("BindPoseCount: " + BindPoseCount);
                    for (int num5 = 0; num5 < BindPoseCount; num5++)
                    {
                        Matrix item3 = new Matrix(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
                        BindPose.Add(item3);
                    }

                    // Read inverse bind pose
                    List<Matrix> InverseBindPose = new List<Matrix>();
                    int InverseBindPoseCount = binaryReader.ReadInt32();
                    Console.WriteLine("InverseBindPoseCount: " + InverseBindPoseCount);
                    for (int num6 = 0; num6 < InverseBindPoseCount; num6++)
                    {
                        Matrix item4 = new Matrix(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
                        InverseBindPose.Add(item4);
                    }

                    // Read animation cliips
                    Dictionary<string, URL> AnimationClips = new Dictionary<string, URL>();
                    int AnimationCount = binaryReader.ReadInt32();
                    Console.WriteLine("AnimationCount: " + AnimationCount);
                    for (int num7 = 0; num7 < AnimationCount; num7++)
                    {
                        string key2 = binaryReader.ReadString();
                        Console.WriteLine("key2[" + num7 + "]: " + key2);
                        double value2 = binaryReader.ReadDouble();
                        Console.WriteLine("value2[" + num7 + "]: " + value2);
                        List<MRL> list6 = new List<MRL>();
                        int num8 = binaryReader.ReadInt32();
                        Console.WriteLine("num8[" + num7 + "]: " + num8);
                        for (int num9 = 0; num9 < num8; num9++)
                        {
                            int i2 = binaryReader.ReadInt32();
                            double value3 = binaryReader.ReadDouble();
                            Matrix p = new Matrix(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
                            list6.Add(new MRL(i2, TimeSpan.FromMilliseconds(value3), p));
                        }
                        AnimationClips.Add(key2, new URL(TimeSpan.FromMilliseconds(value2), list6));
                    }


                    H = new KRL(AnimationClips, BindPose, InverseBindPose, SkeletonHierarchy, BoneIndecies);

                    ObjFile objFile = new ObjFile();
                    foreach (EW ew in VertexBuffer)
                    {
                        objFile.Vertices.Add(new ObjVertex(ew.SVPosition.X, ew.SVPosition.Y, ew.SVPosition.Z));
                        objFile.VertexNormals.Add(new ObjVector3(ew.Normal.X, ew.Normal.Y, ew.Normal.Z));
                        objFile.TextureVertices.Add(new ObjVector3(ew.TexCoord0.X, -ew.TexCoord0.Y + 1, 0));
                    }

                    foreach(var faceVerts in IndexBuffer.Batch(3))
                    {
                        ObjFace face = new ObjFace();
                        foreach(var faceVert in faceVerts)
                        {
                            ObjTriplet v = new ObjTriplet(faceVert + 1, faceVert + 1, faceVert + 1);
                            face.Vertices.Add(v);
                        }
                        objFile.Faces.Add(face);
                    }

                    objFile.WriteTo(@outputFilePath);
                }
            }
            finally
            {
                if(fs != null)
                {
                    fs.Dispose();
                }
            }

            return 0;
        }
    }
}
