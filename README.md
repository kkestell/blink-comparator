# Blink Comparator

Blink Comparator is a C# program for monitoring changes to a specific HTML element on a web page and sending notifications via Telegram when changes are detected.

## Description

The program retrieves the content of the specified HTML element from the specified URL. If the content has changed relative to the last check, a notification is sent to a specified Telegram chat. The URL, HTML element selector, and Telegram settings are read from a JSON configuration file.

## Options

No command-line options are required.

## Environment Variables

* `BLINK_COMPARATOR_CONFIG`: This environment variable can be set to the path of the configuration file. If not set, the configuration file at `~/.config/blink-comparator/config.json` is used.
* `BLINK_COMPARATOR_STATE`: This environment variable can be set to the path of the state file. If not set, the state file at `~/.local/state/blink-comparator/state.json` is used.

## Configuration File

The configuration file is a JSON file that specifies the settings for the program. It includes the following properties:

* `Url` (string, required): The URL of the page to monitor.
* `HtmlSelector` (string, required): The CSS selector of the HTML element to monitor.
* `ApiKey` (string, required): The API key for the Telegram bot.
* `ChatId` (int64, required): The ID of the Telegram chat where notifications will be sent.

Example configuration file:

```json
{
  "Url": "http://example.com",
  "HtmlSelector": ".content",
  "ApiKey": "yourtelegramapikey",
  "ChatId": 1234567890
}
