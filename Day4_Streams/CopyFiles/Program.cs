using System;
using System.Net;
using System.IO;
using System.Text;

namespace FileStreams
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string source = "source.txt";
            string destination = "destination.txt";


            ByteCopy(source, destination);
            BlockCopy(source, destination);
            LineCopy(source, destination);
            MemoryBufferCopy(source, destination);
            WebClient();
            Console.ReadKey();
        }

        public static void ByteCopy(string source, string destin)
        {
            int bytesCounter = 0;
            using (var sourceStream = new FileStream(source, FileMode.OpenOrCreate))
            using (var destinStream = new FileStream(destin, FileMode.Append))
            {
                int b;
                while ((b = sourceStream.ReadByte()) != -1) // TODO: read byte
                {
                    destinStream.WriteByte((byte)b);
                    bytesCounter++;
                }
            }
            Console.WriteLine("ByteCopy() done. Total bytes: {0}", bytesCounter);
        }

        public static void BlockCopy(string source, string destin)
        {
            using (var sourceStream = new FileStream(source, FileMode.OpenOrCreate))
            using (var destinStream = new FileStream(destin, FileMode.Append))
            {
                byte[] buffer = new byte[1024];
                int bytesRead = 0;
                do
                {
                    bytesRead = sourceStream.Read(buffer, bytesRead, 1024); // TODO: read in buffer
                    Console.WriteLine("BlockCopy(): writing {0} bytes.", bytesRead);
                    destinStream.Write(buffer,0,bytesRead); // TODO: write to buffer
                }
                while (bytesRead == buffer.Length);
            }

        }

        public static void LineCopy(string source, string destin)
        {
            int linesCount = 0;

            // TODO: implement copying lines using StreamReader/StreamWriter.

            using (var sourceStream = new FileStream(source, FileMode.OpenOrCreate))
            using (var destinStream = new FileStream(destin, FileMode.Append))
            {
                using (var streamReader = new StreamReader(sourceStream))
                using (var streamWriter = new StreamWriter(destinStream))
                {
                    string line;
                    while (true)
                    {
                        linesCount++;
                        if ((line = streamReader.ReadLine()) == null) // TODO: read line
                        {
                            break;
                        }
                        streamWriter.WriteLine(line); // TODO: write line

                    }
                }
            }
            Console.WriteLine("LineCopy(): {0} lines.", linesCount);
        }

        public static void MemoryBufferCopy(string source, string destin)
        {

            var stringBuilder = new StringBuilder();

            string content;

            using (var textReader = (TextReader)new StreamReader(new FileStream(source, FileMode.Open))) // TODO: use StreamReader here
            {
                // TODO: read entire file
                content = textReader.ReadToEnd();
            }

            using (var sourceReader = new StringReader(content)) // TODO: Use StringReader here with content
            using (var sourceWriter = new StringWriter(stringBuilder)) // TODO: Use StringWriter here with stringBuilder
            {
                string line = null;

                do
                {
                    line = sourceReader.ReadLine(); // TODO: read line
                    if (line != null)
                    {
                        sourceWriter.WriteLine(line); // TODO: write line
                    }

                } while (line != null);
            }

            Console.WriteLine("MemoryBufferCopy(): chars in StringBuilder {0}", stringBuilder.Length);

            const int blockSize = 1024;

            using (var stringReader = new StringReader(stringBuilder.ToString())) // TODO: Use StringReader to read from stringBuilder.
            using (var memoryStream = new MemoryStream(blockSize))
            using (var streamWriter = new StreamWriter(memoryStream)) // TODO: Compose StreamWriter with memory stream.
            using (var destinStream = new FileStream(destin, FileMode.Append)) // TODO: Use file stream.
            {
                char[] buffer = new char[blockSize];
                int bytesRead;

                do
                {
                    bytesRead = stringReader.ReadBlock(buffer,0, blockSize); // TODO: Read block from stringReader to buffer.
                    streamWriter.WriteLine(buffer); // TODO: Write buffer to streamWriter.

                    //TODO: After implementing everythin check the content of NewTextFile. What's wrong with it, and how this may be fixed?

                     // TODO: write memoryStream.GetBuffer() content to destination stream.
                    destinStream.Write(memoryStream.GetBuffer(), 0, blockSize);
                }
                while (bytesRead == blockSize);
            }
        }

        public static void WebClient()
        {
            WebClient webClient = new WebClient();
            using (var stream = webClient.OpenRead("http://google.com"))
            {

                Console.WriteLine("WebClient(): CanRead = {0}", stream.CanRead); // TODO: print if it is possible to read from the stream
                Console.WriteLine("WebClient(): CanWrite = {0}", stream.CanWrite); // TODO: print if it is possible to write to the stream
                Console.WriteLine("WebClient(): CanSeek = {0}", stream.CanSeek); // TODO: print if it is possible to seek through the stream

                // TODO: Save steam content to "google_request.txt" file.
                using (FileStream fs = new FileStream("google_request.txt", FileMode.Create))
                {
                    using (StreamReader sr = new StreamReader(stream))
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.Write(sr.ReadToEnd());
                    }
                }
            }
        }
    }
}