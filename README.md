# Blink Comparator

Blink Comparator monitors a website and sends a Telegram message when it changes.

## Building

To build Blink Comparator, clone the repository and navigate to the project directory:

```console
$ git clone https://github.com/kkestell/blink-comparator.git blink-comparator
$ cd blink-comparator
```

And build:

```console
$ go build
```

or

```console
$ make
```

## Configuration

Blink Comparator is configured using a JSON file. The default configuration file is `config.json` in the same directory as the executable.

Example:

```json
{
  "stateFile": "state.json",
  "url": "https://example.com",
  "apiKey": "1679546312:FFDGH_Z5v_4JnEJauahsjfdhawF28Q",
  "chatID": 1457865941
}
```

## Running as a systemd service

Blink Comparator can be run as a systemd service, which allows it to run in the background and automatically start on boot.

### Create Service

To create a systemd service, create a file called `blink-comparator.service` in the `~/.config/systemd/user/` directory:

```console
$ mkdir -p ~/.config/systemd/user
$ nano ~/.config/systemd/user/blink-comparator.service
```

In the file, add the following configuration:

```ini
[Unit]
Description=Blink Comparator

[Service]
Type=oneshot
ExecStart=%h/blink-comparator/blink-comparator
WorkingDirectory=%h/blink-comparator
```

### Create Timer

To schedule the service to run at regular intervals, create a file called bc.timer in the `~/.config/systemd/user/` directory:

```console
$ nano ~/.config/systemd/user/blink-comparator.timer
```

In the file, add the following configuration:

```ini
[Unit]
Description=Blink Comparator timer

[Timer]
OnUnitActiveSec=15min
OnBootSec=10s

[Install]
WantedBy=timers.target
```

### Enable and start

Enable and start the service and timer:

```console
$ systemctl --user daemon-reload
$ systemctl --user enable blink-comparator.timer
$ systemctl --user start blink-comparator.timer
```

### Lingering

By default, systemd user instances are started when a user logs in and stopped when their last session ends. However, to ensure that Blink Comparator is always running, even when no user is logged in, you can enable "lingering" for the current user:

```console
$ loginctl enable-linger $(whoami)
```

### Check status

Check the status of the service and timer:

```console
$ systemctl --user status blink-comparator.timer
$ systemctl --user status blink-comparator.service
$ systemctl --user list-timers --all
```

### Logs

View the logs for the service and timer:

```console
$ journalctl --user-unit blink-comparator.timer
$ 
```

## Notes

```console
$ pip install beautifulsoup4 python-telegram-bot requests
$ pip freeze > requirements.txt
```

## Name

> A blink comparator is a viewing apparatus formerly used by astronomers to find differences between two photographs of the night sky. It permits rapid switching from viewing one photograph to viewing the other, "blinking" back and forth between the two images taken of the same area of the sky at different times. This allows the user to more easily spot objects in the night sky that have changed position or brightness.

See: https://en.wikipedia.org/wiki/Blink_comparator