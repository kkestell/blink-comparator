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

The systemd user instance runs when a user logs in and stops when their last session ends. But sometimes, it's needed to start the systemd user instance right after boot and keep it running even after the user logs out. This is called "lingering," and you can enable it for a specific user using the command:

```console
$ loginctl enable-linger $(whoami)
```

### Check status

```console
$ systemctl --user status bc.timer
$ systemctl --user status bc.service
$ systemctl --user list-timers --all
$ journalctl --user-unit bc.timer
$ journalctl --user-unit bc.service
```

## Notes

```console
$ pip install beautifulsoup4 python-telegram-bot requests
$ pip freeze > requirements.txt
```