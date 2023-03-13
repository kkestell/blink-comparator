## Dependencies

```console
$ sudo apt install python3-venv
$ python3 -m venv venv
$ source venv/bin/activate
$ pip install -r requirements.txt
```

### Service

```console
$ nano ~/.config/systemd/user/bc.service
```

```ini
[Unit]
Description=Blink Comparator

[Service]
Type=oneshot
ExecStart=%h/bc/venv/bin/python %h/bc/bc.py
Environment=URL=https://example.com
Environment=TELEGRAM_API_KEY=123456789:ABC-DEF1234ghIkl-zyx57W2v1u123ew11
Environment=TELEGRAM_CHAT_ID=123456789
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
$ pip install beautifulsoup4 requests python-telegram-bot
$ pip freeze > requirements.txt
```