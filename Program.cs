using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Needleman
{
    internal class Program
    {
        static string GenerateRandomDNASequence(Random random, int length)
        {
            char[] dnaBases = { 'A', 'T', 'C', 'G' };
            char[] dnaSequence = new char[length];

            for (int i = 0; i < length; i++)
            {
                int randomIndex = random.Next(dnaBases.Length);
                dnaSequence[i] = dnaBases[randomIndex];
            }

            return new string(dnaSequence);
        }
        static void Main(string[] args)
        {
            Random random = new Random();
            int length = 1000;
            int length1 = 1000;
            string refSeq= GenerateRandomDNASequence(random, length);
            string alignSeq = GenerateRandomDNASequence(random, length1);
            //string refSeq = "CTCGCAGC";
            //string alignSeq = "CATTCAC";
            int refSeqCnt = refSeq.Length + 1;
            int alineSeqCnt = alignSeq.Length + 1;

            int[,] scoringMatrix = new int[alineSeqCnt, refSeqCnt];

            Stopwatch cronometru = new Stopwatch();
            cronometru.Start();

            //Initialization Step - filled with 0 for the first row and the first column of matrix
            for (int i = 0; i < alineSeqCnt; i++)
            {
                scoringMatrix[i, 0] = 0;
            }

            for (int j = 0; j < refSeqCnt; j++)
            {
                scoringMatrix[0, j] = 0;
            }
            //Matrix Fill Step
            for (int i = 1; i < alineSeqCnt; i++)
            {
                for (int j = 1; j < refSeqCnt; j++)
                {
                    int scroeDiag = 0;
                    if (refSeq.Substring(j - 1, 1) == alignSeq.Substring(i - 1, 1))
                        scroeDiag = scoringMatrix[i - 1, j - 1] +10;
                    else
                        scroeDiag = scoringMatrix[i - 1, j - 1] +5;

                    int scroeLeft = scoringMatrix[i, j - 1] +2;
                    int scroeUp = scoringMatrix[i - 1, j] +2;

                    int maxScore = Math.Max(Math.Max(scroeDiag, scroeLeft), scroeUp);

                    scoringMatrix[i, j] = maxScore;
                }
            }
            //Traceback Step
            char[] alineSeqArray = alignSeq.ToCharArray();
            char[] refSeqArray = refSeq.ToCharArray();

            string AlignmentA = string.Empty;
            string AlignmentB = string.Empty;
            int m = alineSeqCnt - 1;
            int n = refSeqCnt - 1;
            while (m > 0 && n > 0)
            {
                int scroeDiag = 0;

                //Remembering that the scoring scheme is +2 for a match, -1 for a mismatch and -2 for a gap
                if (alineSeqArray[m - 1] == refSeqArray[n - 1])
                    scroeDiag = 10;
                else
                    scroeDiag = 5;

                if (m > 0 && n > 0 && scoringMatrix[m, n] == scoringMatrix[m - 1, n - 1] + scroeDiag)
                {
                    AlignmentA = refSeqArray[n - 1] + AlignmentA;
                    AlignmentB = alineSeqArray[m - 1] + AlignmentB;
                    m = m - 1;
                    n = n - 1;
                }
                else if (n > 0 && scoringMatrix[m, n] == scoringMatrix[m, n - 1] +2)
                {
                    AlignmentA = refSeqArray[n - 1] + AlignmentA;
                    AlignmentB = "-" + AlignmentB;
                    n = n - 1;
                }
                else //if (m > 0 && scoringMatrix[m, n] == scoringMatrix[m - 1, n] + -2)
                {
                    AlignmentA = "-" + AlignmentA;
                    AlignmentB = alineSeqArray[m - 1] + AlignmentB;
                    m = m - 1;
                }
            }

            cronometru.Stop();
            double ms = cronometru.ElapsedMilliseconds;
            Console.Write(ms + "\n");

            Console.Write(refSeq + "\n");
            Console.Write(alignSeq + "\n\n");

            Console.Write(AlignmentA+"\n");
            Console.Write(AlignmentB);
            Console.Read();
        }
    }
}
