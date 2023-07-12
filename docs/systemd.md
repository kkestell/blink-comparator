# Running as a Systemd Service

Blink Comparator can be run as a Systemd service, which allows it to run in the background and automatically start on boot.

### Create Service

To create a Systemd service, create a file called `blink-comparator.service` in the `~/.config/systemd/user/` directory:

```console
mkdir -p ~/.config/systemd/user
nano ~/.config/systemd/user/blink-comparator.service
```

In the file, add the following configuration:

```ini
[Unit]
Description=Blink Comparator

[Service]
Type=oneshot
ExecStart=%h/.local/bin/blink-comparator 
WorkingDirectory=%h/.local/bin
```

### Create Timer

To schedule the service to run at regular intervals, create a file called `blink-comparator.timer` in the `~/.config/systemd/user/` directory:

```console
nano ~/.config/systemd/user/blink-comparator.timer
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
systemctl --user daemon-reload
systemctl --user enable blink-comparator.timer
systemctl --user start blink-comparator.timer
```

### Lingering

By default, Systemd user instances are started when a user logs in and stopped when their last session ends. However, to ensure that Blink Comparator is always running, even when no user is logged in, you can enable "lingering" for the current user:

```console
loginctl enable-linger $(whoami)
```

### Check status

Check the status of the service and timer:

```console
systemctl --user status blink-comparator.timer
systemctl --user status blink-comparator.service
systemctl --user list-timers --all
```

### Logs

View the logs for the service and timer:

```console
journalctl --user-unit blink-comparator.service
journalctl --user-unit blink-comparator.timer
```