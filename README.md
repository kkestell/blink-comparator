# Blink Comparator

Monitor a website and send a Telegram message when it changes.

## Dependencies

```console
$ sudo apt install python3-pip python3-venv
```

```console
$ git clone https://github.com/kkestell/blink-comparator.git bc
$ cd bc
```

```console
$ python3 -m venv venv
$ source venv/bin/activate
$ pip install -r requirements.txt
```

## Systemd

### Service

```console
$ mkdir -p ~/.config/systemd/user
```

```console
$ nano ~/.config/systemd/user/bc.service
```

```ini
[Unit]
Description=Blink Comparator

[Service]
Type=oneshot
ExecStart=%h/bc/venv/bin/python %h/bc/main.py
WorkingDirectory=%h/bc
```

### Timer

```console
$ nano ~/.config/systemd/user/bc.timer
```

```ini
[Unit]
Description=Binary Comparator timer

[Timer]
OnUnitActiveSec=15min
OnBootSec=10s

[Install]
WantedBy=timers.target
```

### Enable and start

```console
$ systemctl --user daemon-reload
$ systemctl --user enable bc.timer
$ systemctl --user start bc.timer
```

"Lingering" is a systemd feature that allows the systemd user instance to start running right after boot and keep running even after the user logs out. Normally, the systemd user instance is started when a user logs in and stopped when their last session ends. However, with lingering enabled, the systemd user instance will remain running in the background, ready for the next user to log in.

This feature is useful in situations where a service or a daemon needs to be constantly running, regardless of whether a user is logged in or not. By enabling lingering for a specific user, you ensure that the necessary background processes are always running, even if no user is currently logged in.

```console
$ loginctl enable-linger $(whoami)
```

### Check status

```console
$ systemctl --user status bc.timer
$ systemctl --user status bc.service
$ systemctl --user list-timers --all
```

### Logs

```console
$ journalctl --user-unit bc.timer
$ journalctl --user-unit bc.service
```

## Notes

```console
$ pip install beautifulsoup4 python-telegram-bot requests
$ pip freeze > requirements.txt
```