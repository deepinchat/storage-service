namespace Deepin.Storage.API.Infrastructure.Helpers;

public class ChecksumHelper
{
    public bool VerifyFileIntegrity(Stream stream, string originalChecksum, CancellationToken cancellationToken = default)
    {
        if (originalChecksum.Length != 8)
        {
            throw new ArgumentException("Invalid checksum format");
        }
        var currentChecksum = GetCRC32Checksum(stream, cancellationToken);
        return string.Equals(currentChecksum, originalChecksum, StringComparison.OrdinalIgnoreCase);
    }
    public static string GetCRC32Checksum(Stream stream, CancellationToken cancellationToken = default)
    {
        stream.Position = 0;
        uint polynomial = 0xEDB88320;
        uint[] table = new uint[256];
        uint temp;

        for (uint i = 0; i < table.Length; i++)
        {
            temp = i;
            for (int j = 8; j > 0; j--)
            {
                if ((temp & 1) == 1)
                    temp = (temp >> 1) ^ polynomial;
                else
                    temp >>= 1;
            }
            table[i] = temp;
        }

        uint crc = 0xFFFFFFFF;
        int ch;
        while ((ch = stream.ReadByte()) != -1)
        {
            if (cancellationToken.IsCancellationRequested)
                throw new OperationCanceledException();

            crc = ((crc >> 8) ^ table[(crc & 0xFF) ^ (byte)ch]);
        }
        return (~crc).ToString("X8");
    }
}
