# S3CopyBucketFiles

Written in C#.
Is a Console Application that copy one file from a S3 bucket to another S3 bucket.
The logic behind the application is to get the most heavy file on the S3 bucket and copy it to another S3 bucket.
Also, it makes convertions from Bytes to MB, iterate the file to rename it, etc.

It requires to have AWS Access key and secrete key and bucket name.

This was developed, mainly for personal use, to copy the largest files from the bucket and paste it to another bucket with a different name.
