using SharpDX;
using Run8Tools.InternalClasses;
using System.Text;
using System.Security.Cryptography;

namespace Run8Tools.Commands
{
    internal class Texture : ICommand
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

            if(!inputFilePath.EndsWith(".tx8"))
            {
                Console.WriteLine("Not a valid texture file");
                return 1;
            }


            string? inputFileDirectory = Path.GetDirectoryName(inputFilePath);
            if (inputFileDirectory == "")
            {
                inputFileDirectory = Directory.GetCurrentDirectory();
            }



            outputFileName = Path.GetFileNameWithoutExtension(inputFilePath) + ".dds";
            outputFilePath = Path.Join(inputFileDirectory, outputFileName);

            try
            {
                if(inputFilePath.Contains("Avatar") || args.Contains("--as-avatar"))
                {
                    Console.WriteLine("Processing as an avatar texture.");
                    return DoWorkAvatarTexture(inputFilePath, outputFilePath);
                } else
                {
                    return DoWork(inputFilePath, outputFilePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine("Command Failed: " + ex.Message);
                return 1;
            }
        }

        public int DoWorkAvatarTexture(string inputFile, string outputFile)
        {
            byte[] H = new byte[32];
            using (FileStream fileStream = File.OpenRead(inputFile))
            {
                try
                {
                    byte[] array = new byte[(int)checked((IntPtr)unchecked(fileStream.Length - 32L))];
                    fileStream.Read(array, 0, (int)fileStream.Length - 32);
                    fileStream.Read(H, 0, 32);
                    Q(array);
                    Q(H);
                    if(Q(H, K(array)))
                    {
                        using (FileStream fileStream1 = File.OpenWrite(outputFile))
                        {
                            fileStream1.Write(array);
                        }
                        return 0;
                    } else
                    {
                        Console.WriteLine("checksum failure");
                        return 1;
                    }
                } catch(Exception message)
                {
                    Console.WriteLine(message);
                    return 1;
                }
            }
        }

        internal static byte[] K(byte[] I)
        {
            MD5 md = MD5.Create();
            byte[] bytes;
            try
            {
                bytes = Encoding.ASCII.GetBytes(BitConverter.ToString(md.ComputeHash(I)).Replace("-", string.Empty));
            }
            finally
            {
                if (md != null)
                {
                    ((IDisposable)md).Dispose();
                }
            }
            return bytes;
        }

        private void Q(byte[] I)
		{
			if (I != null)
			{
				if (I.Length != 0)
				{
					for (int i = 0; i < I.Length; i++)
					{
						I[i] = R((int)(I[i] + 96));
					}
					return;
				}
			}
		}

        internal static bool Q(byte[] I, byte[] L)
        {
            if (I != null)
            {
                if (L == null)
                {
                }
                else
                {
                    if (I.Length == L.Length)
                    {
                        for (int i = 0; i < I.Length; i++)
                        {
                            if (I[i] != L[i])
                            {
                                return false;
                            }
                        }
                        return true;
                    }
                    return false;
                }
            }
            return false;
        }

        internal static byte R(int I)
        {
            if (I > 255)
            {
                return (byte)(I - 256);
            }
            if (I < 0)
            {
                return (byte)(I + 256);
            }
            return (byte)I;
        }

        public int DoWork(string inputFile, string outputFile)
        {
            List<NG> L;
            float BsRadius = 0f;

            using(FileStream fs = File.OpenRead(inputFile))
            {
                using(BinaryReader binaryReader  = new BinaryReader(fs))
                {
                    bool flag = false;
                    int num = 1;
                    int num2 = binaryReader.ReadInt32();
                    Console.WriteLine("num2: " + num2);
                    if (num2 == -969696)
                    {
                        num = binaryReader.ReadInt32();
                        Console.WriteLine("num: " + num);
                        flag = true;
                    } else if (num2 == -969697)
                    {
                        num = binaryReader.ReadInt32();
                        Console.WriteLine("num: " + num);
                        var F = new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
                        flag = true;
                    } else
                    {
                        binaryReader.BaseStream.Position = 0L;
                    }
                    L = new List<NG>(num);
                    for (int i = 0; i < num; i++)
                    {
                        NG ng = new NG();
                        if(flag)
                        {
                            ng.L = binaryReader.ReadString();
                            ng.P = binaryReader.ReadString();
                            ng.K = new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
                            ng.G = new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
                            ng.D = Quaternion.RotationMatrix(Matrix.RotationYawPitchRoll(MathUtil.DegreesToRadians(binaryReader.ReadSingle()), MathUtil.DegreesToRadians(binaryReader.ReadSingle()), MathUtil.DegreesToRadians(binaryReader.ReadSingle())));
                            Matrix.Scaling(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
                            Quaternion.RotationMatrix(Matrix.RotationYawPitchRoll(MathUtil.DegreesToRadians(binaryReader.ReadSingle()), MathUtil.DegreesToRadians(binaryReader.ReadSingle()), MathUtil.DegreesToRadians(binaryReader.ReadSingle())));
                            Quaternion.RotationMatrix(Matrix.RotationYawPitchRoll(MathUtil.DegreesToRadians(binaryReader.ReadSingle()), MathUtil.DegreesToRadians(binaryReader.ReadSingle()), MathUtil.DegreesToRadians(binaryReader.ReadSingle())));
                            ng.U = new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
                            ng.Q = Quaternion.RotationMatrix(Matrix.RotationYawPitchRoll(MathUtil.DegreesToRadians(binaryReader.ReadSingle()), MathUtil.DegreesToRadians(binaryReader.ReadSingle()), MathUtil.DegreesToRadians(binaryReader.ReadSingle())));
                            Matrix.Scaling(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
                            int num3 = binaryReader.ReadInt32();
                            Matrix[] array = new Matrix[num3];
                            
                            for(int j = 0; j < num3; j++)
                            {
                                if(ng.J == null)
                                {
                                    ng.J = new GG
                                    {
                                        I = new Quaternion[num3],
                                        L = new Vector3[num3]
                                    };
                                }
                                array[j] = new Matrix(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
                            }

                            int num4 = binaryReader.ReadInt32();
                            Console.WriteLine("num4: " + num4);
                            Matrix[] array2 = new Matrix[num4];
                            for (int k = 0; k < num4; k++)
                            {
                                array2[k] = new Matrix(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
                            }
                            if (num4 != num3)
                            {
                                //ng.J = new GG();
                                Console.WriteLine("What are we suppose to do here? num4 != num3");
                                return 1;
                            }
                            else
                            {
                                for(int l = 0; l < num4; l++)
                                {
                                    ng.J.I[l] = Quaternion.RotationMatrix(array2[l]);
                                    ng.J.L[l] = array[l].TranslationVector;
                                }
                            }
                        } else
                        {
                            ng.L = "";
                            ng.P = "";
                            ng.K = Vector3.Zero;
                        }
                        List<VertexPositionNormalTexture> list = new List<VertexPositionNormalTexture>();
                        int num5 = binaryReader.ReadInt32() / 7;
                        Console.WriteLine("num5: " + num5);

                        for (int m = 0; m < num5; m++)
                        {
                            VertexPositionNormalTexture vertexPositionNormalTexture = default(VertexPositionNormalTexture);
                            binaryReader.ReadSingle();
                            vertexPositionNormalTexture.Position.X = binaryReader.ReadSingle() * 63.7f - ng.K.X;
                            vertexPositionNormalTexture.Normal.Y = binaryReader.ReadSingle() / -1.732f;
                            vertexPositionNormalTexture.Position.Z = binaryReader.ReadSingle() / 16f - ng.K.Z;
                            vertexPositionNormalTexture.TextureCoordinate.X = binaryReader.ReadSingle() / 4.8f;
                            vertexPositionNormalTexture.Normal.X = binaryReader.ReadSingle() / 10.962f;
                            binaryReader.ReadSingle();
                            vertexPositionNormalTexture.Normal.Z = binaryReader.ReadSingle() / 11.432f;
                            vertexPositionNormalTexture.TextureCoordinate.Y = binaryReader.ReadSingle() / 9.6f;
                            vertexPositionNormalTexture.Position.Y = -binaryReader.ReadSingle() * 6f - ng.K.Y;
                            list.Add(vertexPositionNormalTexture);
                            float num6 = Math.Max(Math.Abs(vertexPositionNormalTexture.Position.X), Math.Max(Math.Abs(vertexPositionNormalTexture.Position.Y), Math.Abs(vertexPositionNormalTexture.Position.Z)));
                            if(num6 > BsRadius)
                            {
                                BsRadius = num6;
                            }
                        }
                        ng.VertexBuffer = list.ToArray();
                        num5 = binaryReader.ReadInt32() + 6;
                        Console.WriteLine("num52: " + num5);
                        List<string> list2 = new List<string>();
                        for (int n = 0; n < num5; n++)
                        {
                            string path = binaryReader.ReadString();
                            Console.WriteLine("path: " + path);
                            list2.Add(Path.GetFileNameWithoutExtension(path));
                        }
                        bool flag2 = binaryReader.ReadBoolean();
                        Console.WriteLine("flag2: " + flag2);
                        int num7 = binaryReader.ReadInt32();
                        Console.WriteLine("num7: " + num7);
                        if(flag2)
                        {
                            int[] array3 = new int[num7];
                            for (int num8 = 0; num8 < num7; num8++)
                            {
                                array3[num8] = (ushort)binaryReader.ReadInt32();
                            }
                            ng.IndexBuffer = array3;
                        } else
                        {
                            int[] array4 = new int[num7];
                            for (int num9 = 0; num9 < num7; num9++)
                            {
                                array4[num9] = binaryReader.ReadInt32();
                            }
                            ng.IndexBuffer = array4;
                        }
                        num5 = binaryReader.ReadInt32() - 9;
                        Console.WriteLine("num53: " + num5);
                        if (num5 == 0)
                        {
                            CW item = new CW
                            {
                                ElementCount = ng.IndexBuffer.Length,
                                P = 0,
                                F = 0,
                            };
                            ng.H.Add(item);
                        } else
                        {
                            object g = new object();
                            lock(g)
                            {
                                for(int num10 = 0; num10 < num7; num10++)
                                {
                                    CW cw = new CW();
                                    binaryReader.ReadSingle();
                                    int index = binaryReader.ReadInt32();
                                    Console.WriteLine("index: " + index);
                                    if (list2.Count > 0)
                                    {
                                        string text = list2[index];
                                        // LOAD TEXTURE
                                    }
                                    cw.ElementCount = binaryReader.ReadInt32();
                                    cw.F = binaryReader.ReadInt32();
                                    cw.P = binaryReader.ReadInt32();
                                    ng.H.Add(cw);
                                }
                            }
                        }
                        L.Add(ng);
                    }
                }
            }
            using(List<NG>.Enumerator enumerator = L.GetEnumerator())
            {
                while(enumerator.MoveNext())
                {
                    NG item = enumerator.Current;
                    if(!string.IsNullOrEmpty(item.P))
                    {
                        item.V = L.Find(new Predicate<NG>(i => L.Contains(i)));
                    }
                    string text4 = item.L.ToLower();
                    if(text4.Contains("wiper"))
                    {
                        item.I = QG.F;
                    } else if(text4.Contains("beacon"))
                    {
                        item.I = QG.O;
                    }
                    else if (text4.Contains("glass"))
                    {
                        NG i2 = item;
                        QG i3;
                        if (!text4.Contains("rain"))
                        {
                            i3 = QG.R;
                        }
                        else
                        {
                            i3 = QG.G;
                        }
                        i2.I = i3;
                    }
                    else if (text4.Contains("holder"))
                    {
                        item.I = QG.R;
                    }
                    else if (text4.Contains("window"))
                    {
                        item.I = O(text4);
                    }
                    else if (text4.Contains("r_door"))
                    {
                        item.I = QG.D;
                    }
                    else if (text4.Contains("f_door"))
                    {
                        item.I = QG.Q;
                    }
                    else if (text4.Contains("carload"))
                    {
                        item.I = QG.P;
                    }
                }
                BsRadius *= 1.2f;
            }

            return 0;
        }

        private QG O(string I)
        {
            if (!I.Contains("engineer"))
            {
                if (I.Contains("driver"))
                {
                    
                }
                else
                {
                    if (I.Contains("fireman") || I.Contains("conductor"))
                    {
                        return QG.H;
                    }
                    if (!I.Contains("window03"))
                    {
                        if (!I.Contains("window04"))
                        {
                            if (!I.Contains("window01"))
                            {
                                if (!I.Contains("window02"))
                                {
                                    return QG.I;
                                }
                            }
                            return QG.C;
                        }
                    }
                    return QG.H;
                }
            }
            return QG.C;
        }
    }
}
