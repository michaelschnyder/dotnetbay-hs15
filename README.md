# dotnetbay - A .NET Auction Solution 
Master: [![Build status](https://ci.appveyor.com/api/projects/status/qp7ueees06ri8agu?svg=true)](https://ci.appveyor.com/project/michaelschnyder/fhnw-dotnetbay)
Fork: [![Build status](https://ci.appveyor.com/api/projects/status/82sl4qpht9atbdeb?svg=true)](https://ci.appveyor.com/project/michaelschnyder/dotnetbay)
Branch: [![Build status](https://ci.appveyor.com/api/projects/status/82sl4qpht9atbdeb/branch/aspnet-webapi?svg=true)](https://ci.appveyor.com/project/michaelschnyder/dotnetbay/branch/aspnet-webapi)

This is a solution branch for the bootstrapped solution from https://github.com/FHNW-dnead/dotnetbay

## ASP.NET Web API Solution

This Branch contains a ASP.NET Standalone version of the DotNetBay-Service based on Entity Framework. There is no Runner which handles auctions. The Web API itself is selfhosted and OWIN-based. You can just start the SelfHost-Commandline-Project.

![](assets/webapi-auction.png)

###Endpoints
The following Endpoints are implemented

**General**
* API Status: `api/status`

**Auctions**
* All Auctions: `GET /api/auctions` (as Json)
* Add Auction: `POST /api/auctions` (as Json)
* Single Auction: `GET /api/auctions/{id}` (as Json)
* Get Image for Auction: `GET api/auctions/{id}/image` (as Json)
* Save Image for Auction: `POST api/auctions/{id}/image` (as Multipart)

**Bids**
* Receive all Bids for Auction: `POST /api/auctions/{auctionId}/bids` (as Json)
* Place a Bid for Auction: `POST /api/auctions/{auctionId}/bids` (as Json)
* Receive specific bid: `GET /api/bids/{transactionId}` (as Json)

## Authors
* Michael Schnyder

## License
Licensed under the MIT License

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
