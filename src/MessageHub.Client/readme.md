## MessageHub.Client


### Příklad použití Odeslání emailu


```csharp
MessageHubClient messageHubClient = new MessageHubClient("API-KEY");
EmailMessage emailMessage = EmailMessageHelper.CreateSingleEmailMessage("text@demo.xyz", "Subject", "Body");
messageHubClient.SendEmailAsyn(emailMessage);
