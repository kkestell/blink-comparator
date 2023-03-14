import asyncio
from bs4 import BeautifulSoup
from dataclasses import dataclass
import json
import requests
from telegram import Bot


state_file = 'state.json'
url        = ''
api_key    = ''
chat_id    = ''


@dataclass
class State:
    content: str = None
    count: int = 0


def load_state():
    try:
        with open(state_file, 'r') as f:
            data = json.load(f)
    except (FileNotFoundError, json.JSONDecodeError):
        return State()
    return State(**data)


def save_state(state):
    with open(state_file, 'w') as f:
        json.dump(state.__dict__, f)


def notify(message):
    bot = Bot(token=api_key)
    asyncio.run(bot.send_message(chat_id=chat_id, text=message))


def fetch():
    r = requests.get(url)
    soup = BeautifulSoup(r.text, 'html.parser')
    ul = soup.find('ul', {'class': 'og-grid'})
    text = ul.get_text()
    text = ' '.join(text.split())
    return text


if __name__ == '__main__':
    state = load_state()
    new = fetch()

    if state.content is None:
        state.content = new
        save_state(state)
        exit(0)

    if new != state.content:
        notify(f"Changes detected! Check it: {url}")
        state.content = new
        state.count = 0
    else:
        if state.count % 4 == 0:
            notify('Still alive!')

        state.count += 1

        if state.count >= 96:
            notify('Still no changes...')
            state.count = 0

    save_state(state)
