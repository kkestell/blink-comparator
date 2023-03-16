# Blink Comparator

Blink Comparator is a Python script that monitors a website and sends a Telegram message when it changes.

## Dependencies

* `python`
* `pip`
* `venv`

### Arch

```console
$ sudo pacman -Syu python python-pip python-virtualenv
```

### Debian

```console
$ sudo apt install python3 python3-pip python3-venv
```

## Installation

To install Blink Comparator, clone the repository and navigate to the project directory:

```console
$ git clone https://github.com/kkestell/blink-comparator.git bc
$ cd bc
```

Then, create a virtual environment and install the required packages:

```console
$ python3 -m venv venv
$ source venv/bin/activate
$ pip install -r requirements.txt
```

## Running as a systemd service

Blink Comparator can be run as a systemd service, which allows it to run in the background and automatically start on boot.

### Create Service

To create a systemd service, create a file called `bc.service` in the `~/.config/systemd/user/` directory:

```console
$ mkdir -p ~/.config/systemd/user
$ nano ~/.config/systemd/user/bc.service
```

In the file, add the following configuration:

```ini
[Unit]
Description=Blink Comparator

[Service]
Type=oneshot
ExecStart=%h/bc/venv/bin/python %h/bc/main.py
WorkingDirectory=%h/bc
```

### Create Timer

To schedule the service to run at regular intervals, create a file called bc.timer in the `~/.config/systemd/user/` directory:

```console
$ nano ~/.config/systemd/user/bc.timer
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
$ systemctl --user enable bc.timer
$ systemctl --user start bc.timer
```

### Lingering

By default, systemd user instances are started when a user logs in and stopped when their last session ends. However, to ensure that Blink Comparator is always running, even when no user is logged in, you can enable "lingering" for the current user:

```console
$ loginctl enable-linger $(whoami)
```

### Check status

Check the status of the service and timer:

```console
$ systemctl --user status bc.timer
$ systemctl --user status bc.service
$ systemctl --user list-timers --all
```

### Logs

View the logs for the service and timer:

```console
$ journalctl --user-unit bc.timer
$ journalctl --user-unit bc.service
```

## Notes

```console
$ pip install beautifulsoup4 python-telegram-bot requests
$ pip freeze > requirements.txt
```

## Name

> A blink comparator is a viewing apparatus formerly used by astronomers to find differences between two photographs of the night sky. It permits rapid switching from viewing one photograph to viewing the other, "blinking" back and forth between the two images taken of the same area of the sky at different times. This allows the user to more easily spot objects in the night sky that have changed position or brightness.

See: https://en.wikipedia.org/wiki/Blink_comparator