using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace GW.Redis
{
    /// <summary>
    /// RedisSharding 一致性Hash处理类
    /// </summary>
    /// <author>kevin</author>
    /// <createDate>2014-04-29</createDate>
    public class RedisSharding
    {

        private SortedList<long, string> ketamaNodes = new SortedList<long, string>();
       // private HashAlgorithm hashAlg;
        private static int numReps = 160;

        public RedisSharding(List<string> nodes)
            : this(nodes, numReps)
        {
        }

        //此处参数与JAVA版中有区别，因为使用的静态方法，所以不再传递HashAlgorithm alg参数
        public RedisSharding(List<string> nodes, int nodeCopies)
        {
            ketamaNodes = new SortedList<long, string>();
            numReps = nodeCopies;
            //对所有节点，生成nCopies个虚拟结点
            foreach (string node in nodes)
            {
                //每四个虚拟结点为一组
                for (int i = 0; i < numReps / 4; i++)
                {
                    //getKeyForNode方法为这组虚拟结点得到惟一名称 
                    byte[] digest = HashAlgorithm.ComputeMd5(node + i);
                    /** Md5是一个16字节长度的数组，将16字节的数组每四个字节一组，分别对应一个虚拟结点，这就是为什么上面把虚拟结点四个划分一组的原因*/
                    for (int h = 0; h < 4; h++)
                    {
                        long m = HashAlgorithm.ComputeHash(digest, h);
                        ketamaNodes[m] = node;
                    }
                }
            }
        }

        public string GetNodeByCacheKey(string cacheKey)
        {
            var digest = HashAlgorithm.ComputeMd5(cacheKey.Trim());
            return GetNodeByHashKey(HashAlgorithm.ComputeHash(digest, 0));
        }

        private string GetNodeByHashKey(long hash)
        {

            //如果找到这个节点，直接取节点，返回   
            if (!ketamaNodes.ContainsKey(hash))
            {
                //得到大于当前key的那个子Map，然后从中取出第一个key，就是大于且离它最近的那个key 说明详见: http://www.javaeye.com/topic/684087
                var tailMap = from coll in ketamaNodes
                              where coll.Key > hash
                              select new { coll.Key };
                hash = tailMap.Count() == 0 ? ketamaNodes.FirstOrDefault().Key : tailMap.FirstOrDefault().Key;
            }
            return ketamaNodes[hash];
        }
    }


    /// <summary>
    /// HashAlgorithm 算法处理类
    /// </summary>
    /// <author>kevin</author>
    /// <createDate>2014-04-29</createDate>
    public class HashAlgorithm
    {

        public static long ComputeHash(byte[] digest, int nTime)
        {
            long rv = ((long)(digest[3 + nTime * 4] & 0xFF) << 24)
                    | ((long)(digest[2 + nTime * 4] & 0xFF) << 16)
                    | ((long)(digest[1 + nTime * 4] & 0xFF) << 8)
                    | ((long)digest[0 + nTime * 4] & 0xFF);
            return rv & 0xffffffffL; /* Truncate to 32-bits */
        }


        /// <summary>
        /// 根据MD5算法 计算出相应的哈希值
        /// </summary>
        /// <param name="key">字符串</param>
        /// <returns>字节数组</returns>
        public static byte[] ComputeMd5(string key)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            var keyBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(key));
            md5.Clear();
            return keyBytes;
        }
    }
}
