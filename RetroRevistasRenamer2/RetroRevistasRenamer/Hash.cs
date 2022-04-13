using System;
using System.Security.Cryptography;
using System.Text;
using RetroRevistasRenamer;

public class Hash
{
	public Hash()
	{
	}

    public String md5(byte[] input)
    {
        using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
        {
            byte[] hashBytes = md5.ComputeHash(input);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }

    public String sha1(byte[] input)
    {
        using (SHA1Managed sha1 = new SHA1Managed())
        {
            var hash = sha1.ComputeHash(input);
            var sb = new StringBuilder(hash.Length * 2);

            foreach (byte b in hash)
            {
                // can be "x2" if you want lowercase
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }
    }
}
