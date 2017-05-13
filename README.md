## Synopsis

Tool that returns all IP address blocks of a given Autonomous System Number (ASN) using [ipinfo.io](https://ipinfo.io).

## Example

Retrieves all IP address blocks from Netflix:

```sh
$ git clone https://github.com/bruno-garcia/IPInfo.IO.IPAddressBlockParser.git
$ cd IPInfo.IO.IPAddressBlockParser
$ dotnet restore
$ dotnet run -- AS2906
108.175.32.0/20
108.175.32.0/24
...
```

## Motivation

_ipinfo.io_ offers a [REST API](https://ipinfo.io/developers) but at the time of writing does not offer a 'GET' IP Address blocks by ASN.
This tool doesn't rely on the HTML structure as it extracts IP blocks in the format 0.0.0.0/0 line by line with Regex. 

## Limitations

Only returning IPv4 blocks.

## Dependencies

Requires [.NET Core 1.1](https://www.microsoft.com/net/download/core)

## License

MIT