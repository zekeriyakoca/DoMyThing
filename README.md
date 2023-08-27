# DoMyThing
Authomize your things online. Crawl websites and process them on your behalf.

## Project Architecture 
![DoMyThings drawio](https://github.com/zekeriyakoca/DoMyThing/assets/35925772/053de993-6629-46a2-b0b9-838d40130a01)

# Componenets
- Azure Function App
- Azure Service Bus
- Azure Blob Storage
- Browserless Service for Browser

# Functions
## Download Subtitle Function
1. Triggered by a message into azure service bus queue. 
2. Crawl website to get download link of proper subtitle
3. Download subtitle
4. Upload file to Blob storage
5. Send a message containing saved blob storage name to queue


# Deployment

Function app is isolated. Deploy accordingly. 
Queues in Azure Service Bus should have been created before running application

## Required configurations to be set to Azure function app when deployed
```json
 "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "BlobStorage": "<Azure Storage Connection String>",
    "BlobStorageSubtitleContainer": "subtitles",
    "DownloadSubtitleQueueName": "download-subtitle",
    "SubtitleDownloadedQueueName": "subtitle-downloaded",
    "ServiceBusConnection": "<Azure Service Bus Connection String>",
    "BrowserlessApiKey": "<Browserless Api Key>"
  }
```

