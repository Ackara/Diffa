using Acklann.Diffa.Resolution;
using System.IO;
using System.Text;

namespace Acklann.Diffa
{
    public static class Diff
    {
        public static void Approve(IApprover approver, object subject)
        {
            if (approver.Approve(subject))
            {
                // Assert.Pass();
            }
            else
            {
                // Assert.Fail();
            }
        }

        public static void Approve(Stream stream, string extenstion = ".txt", Encoding encoding = default)
        {
        }

        public static void Approve(string text, string extension = ".txt", Encoding encoding = default)
        {
            throw new System.NotImplementedException();
        }

        public static void ApproveFile(string text)
        {
            throw new System.NotImplementedException();
        }
    }
}