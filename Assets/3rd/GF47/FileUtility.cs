/***************************************************************
 * @File Name       : FileUtility
 * @Author          : GF47
 * @Description     : 文件工具
 * 当前包含获取文件的md5值
 * @Date            : 2017/8/1/星期二 11:50:56
 * @Edit            : none
 **************************************************************/

using System;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

namespace GF47RunTime
{
    public static class FileUtility
    {
        public static string GetFileHash(string path)
        {
            string fileMD5;
            try
            {
                FileStream fs = new FileStream(path, FileMode.Open);
                int len = (int)fs.Length;
                byte[] data = new byte[len];
                fs.Read(data, 0, len);
                fs.Close();
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] result = md5.ComputeHash(data);
                fileMD5 = "";
                for (int i = 0; i < result.Length; i++)
                {
                    fileMD5 += System.Convert.ToString(result[i], 16);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                fileMD5 = "生成错误";
            }

            return fileMD5;
        }
    }
}
