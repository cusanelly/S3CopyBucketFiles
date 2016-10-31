using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
//using System.Web;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace S3Tranfers.Models
{
    public class HomeModels
    {
        private string _accessKey = ConfigurationSettings.AppSettings.Get("accesskey");
        private string _secretKey = ConfigurationSettings.AppSettings.Get("secretkey");
        //private IEnumerable<HttpPostedFileBase> _files { get; set; }
        private string _bucketName { get; set; }
        public int cantidad { get; set; }
        public string filename { get; set; }
        public string message { get; set; }
        public HomeModels()
        {
            _setBucketName("");
        }
        //public HomeModels(HttpPostedFileBase file, string bucketname = "")
        //{
        //    _files = _files.Concat(new[] { file });
        //    _setBucketName(bucketname);
        //    storeFiles();
        //}
        //public HomeModels(IEnumerable<HttpPostedFileBase> files, string bucketname = "")
        //{
        //    _setBucketName(bucketname);
        //    this._files = files;
        //    storeFiles();
        //}
        private void _setBucketName(string bucketname)
        {
            this._bucketName = String.IsNullOrEmpty(bucketname)
                ? ConfigurationSettings.AppSettings.Get("bucketname")
                : bucketname;
        }
        public string copyFiles()
        {
            AmazonS3Client client = getConfig();
            S3Object file = getFile();
            Console.WriteLine($"Bucket Name: {file.BucketName}\n File Key: {file.Key}\n File Size: {file.Size}\n File Tags: {file.ETag}\n File Storage Class: {file.StorageClass}");
            // Split filename for later rename
            string[] name = file.Key.Split('.');
            for (int i = 0; i <= cantidad; i++)
            {
                CopyObjectRequest item = new CopyObjectRequest()
                {
                    SourceBucket = file.BucketName,
                    DestinationBucket = file.BucketName,
                    SourceKey = file.Key,
                    //Rename filename
                    DestinationKey = $"user33_v{i}.{name[1]}"
                };
                client.CopyObject(item);
            }
            message = $"{cantidad} of files have been copied to {file.BucketName} S3 Bucket.";   
            return message;
        }
        private S3Object getFile()
        {
            AmazonS3Client client = getConfig();
            ListObjectsV2Request request = new ListObjectsV2Request()
            {
                BucketName = _bucketName
            };
            ListObjectsV2Response list = client.ListObjectsV2(request);
            long data = 0;
            S3Object file = new S3Object();
            foreach (var item in list.S3Objects)
            {
                if (data < item.Size)
                {
                    file = item;
                    data = item.Size;
                }
            }
            // Convert Bytes to MB
            double t = (file.Size / 1024f) / 1024f;
            // Check how many files we need to achieve x quantity of GB (This case 4.9GB)
            t = 4900 / t;
            // Round number and converted to Int
            cantidad = Convert.ToInt32(Math.Round(t));
            // Return S3 Object
            return file;
        }
        private AmazonS3Client getConfig()
        {
            AmazonS3Config config = new AmazonS3Config()
            {
                BufferSize = 1048576,
                Timeout = new TimeSpan(0, 2, 0),
                RegionEndpoint = RegionEndpoint.USEast1
            };
            AmazonS3Client client = new AmazonS3Client(_accessKey, _secretKey, config);
            return client;
        }
        //private void storeFiles()
        //{
        //    AmazonS3Config config = new AmazonS3Config() {
        //        BufferSize = 1048576,
        //        Timeout = new TimeSpan(0,2,0),
        //        RegionEndpoint = RegionEndpoint.USEast1                              
        //    };
        //    AmazonS3Client client = new AmazonS3Client(_accessKey, _secretKey,config);
        //    foreach (var item in _files)
        //    {
        //        if (item != null && item.ContentLength > 0)
        //        {
        //            try
        //            {
        //                PutObjectRequest file = new PutObjectRequest()
        //                {
        //                    BucketName = _bucketName,
        //                    InputStream = item.InputStream,
        //                    CannedACL = S3CannedACL.PublicReadWrite,
        //                    Key = item.FileName
        //                };
        //                var response = client.PutObject(file);                                             
        //                statusMessage.message += String.Format("FileName: {0}; Status:Uploaded \n", item.FileName);
        //            }
        //            catch (Exception ex)
        //            {
        //                statusMessage.message += String
        //                    .Format("Message: {0}; InnerException: {1}; FileName: {2} \n",
        //                    ex.Message, ex.InnerException, item.FileName);
        //            }
        //        }
        //    }
        //}

    }
    public class statusMessage
    {
        public static bool status { get; set; }
        public static string message { get; set; }
    }
}