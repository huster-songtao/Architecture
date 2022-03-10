using System.Text;
using System.Security.Cryptography;

namespace Architecture
{
    public static class EncryptExtensions
    {
        /// <summary>
        /// 使用 SHA256 哈希函数计算基于哈希值的消息验证代码 (HMAC)。
        /// HMAC 进程将密钥与消息数据混合使用，将结果与哈希函数进行哈希处理，再次将哈希值与密钥混合，然后再次应用哈希函数。
        /// HAMC主要应用在身份验证，服务器对访问者进行鉴权，确定通过不安全通道发送的消息是否已被篡改，发送方和接收方共享密钥。
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="secret">密钥</param>
        /// <returns>返回消息摘要，输出哈希的长度为 256 位。</returns>
        public static string HMACSHA256(this string message, string secret)
        {
            //Check.Argument.IsNotEmpty(srcString, nameof(srcString));
            //Check.Argument.IsNotEmpty(key, nameof(key));
            secret ??= string.Empty;

            var key = Encoding.UTF8.GetBytes(secret);
            using var hmac = new HMACSHA256(key);
            hmac.Initialize();

            byte[] buffer = Encoding.UTF8.GetBytes(message);
            // Compute the hash of the input message.
            byte[] hash = hmac.ComputeHash(buffer);
            return BitConverter.ToString(hash).Replace("-", "");
        }
    }
}
