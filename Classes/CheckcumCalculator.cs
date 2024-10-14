using System;

public class ChecksumCalculator
{
    public static byte CalculateChecksum(string buffer)
    {
        byte checksum = 0;

        // Convert string to byte array
        byte[] byteArray = System.Text.Encoding.ASCII.GetBytes(buffer);

        // Perform XOR on each byte
        foreach (byte b in byteArray)
        {
            checksum ^= b;
        }

        return checksum;
    }

}
