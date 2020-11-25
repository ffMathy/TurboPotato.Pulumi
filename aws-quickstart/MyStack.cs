using System.IO;
using Pulumi;
using Pulumi.Aws.S3;
using Pulumi.Aws.S3.Inputs;

class MyStack : Stack
{
    public MyStack()
    {
        // Create an AWS resource (S3 Bucket)
        var bucket = new Bucket("my-bucket", new BucketArgs
        {
            Website = new BucketWebsiteArgs
            {
                IndexDocument = "index.html"
            }
        });

        var filePath = Path.GetFullPath("./site/index.html");
        var htmlString = File.ReadAllText(filePath);

        var bucketObject = new BucketObject("index.html", new BucketObjectArgs
        {
            Acl = "public-read",
            ContentType = "text/html",
            Bucket = bucket.BucketName,
            Content = htmlString,
        });

        // Export the name of the bucket
        this.BucketName = bucket.Id;
        this.BucketEndpoint = Output.Format($"http://{bucket.WebsiteEndpoint}");
    }

    [Output] public Output<string> BucketName { get; set; }
    [Output] public Output<string> BucketEndpoint { get; set; }
}
