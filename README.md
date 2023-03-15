# Blink Comparator

Blink Comparator is a Python script that monitors a website and sends a Telegram message when it changes. This README file provides instructions on how to set up and run Blink Comparator as a systemd service on a Linux machine.

## Dependencies

Before installing and running Blink Comparator, make sure that you have installed the following dependencies:

* Python 3
* pip3
* venv

To install these dependencies, run the following command in the terminal:

```console
$ sudo apt install python3-pip python3-venv
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

### Service

To create a systemd service, create a file called bc.service in the ~/.config/systemd/user/ directory:

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

This configuration specifies that the service runs a Python script called main.py located in the ~/bc directory using the virtual environment created earlier.

### Timer

To schedule the service to run at regular intervals, create a file called bc.timer in the ~/.config/systemd/user/ directory:

```console
$ nano ~/.config/systemd/user/bc.timer
```

In the file, add the following configuration:

```ini
[Unit]
Description=Binary Comparator timer

[Timer]
OnUnitActiveSec=15min
OnBootSec=10s

[Install]
WantedBy=timers.target
```

This configuration specifies that the service should be run every 15 minutes (`OnUnitActiveSec=15min`) and that it should start 10 seconds after boot (`OnBootSec=10s`).

### Enable and start

To enable and start the service and timer, run the following commands:

### Enable and start

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

To check the status of the service and timer, run the following commands:

```console
$ systemctl --user status bc.timer
$ systemctl --user status bc.service
$ systemctl --user list-timers --all
```

### Logs

To view the logs for the service and timer, run the following commands:

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