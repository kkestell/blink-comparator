package main

import (
	"encoding/json"
	"io/ioutil"
	"net/http"
	"os"
	"strings"

	"github.com/PuerkitoBio/goquery"
	tgbotapi "github.com/go-telegram-bot-api/telegram-bot-api"
)

type Config struct {
	StateFile string `json:"stateFile"`
	URL       string `json:"url"`
	ApiKey    string `json:"apiKey"`
	ChatID    int64  `json:"chatID"`
}

type State struct {
	Content string `json:"content"`
	Count   int    `json:"count"`
}

var config Config

func main() {
	loadConfig()

	state := loadState(config.StateFile)
	newContent := fetch(config.URL)

	if state.Content == "" {
		state.Content = newContent
		saveState(config.StateFile, state)
		os.Exit(0)
	}

	if newContent != state.Content {
		notify("Changes detected! Check it: "+config.URL, config.ApiKey, config.ChatID)
		state.Content = newContent
		state.Count = 0
	} else {
		state.Count++
		if state.Count >= 96 {
			notify("Still no changes...", config.ApiKey, config.ChatID)
			state.Count = 0
		}
	}

	saveState(config.StateFile, state)
}

func loadConfig() {
	data, err := ioutil.ReadFile("config.json")
	if err != nil {
		panic(err)
	}

	err = json.Unmarshal(data, &config)
	if err != nil {
		panic(err)
	}
}

func loadState(stateFile string) State {
	var state State

	data, err := ioutil.ReadFile(stateFile)
	if err != nil {
		return state
	}
	err = json.Unmarshal(data, &state)
	if err != nil {
		return State{}
	}

	return state
}

func saveState(stateFile string, state State) {
	data, _ := json.Marshal(state)
	_ = ioutil.WriteFile(stateFile, data, 0644)
}

func notify(message string, apiKey string, chatID int64) {
	bot, _ := tgbotapi.NewBotAPI(apiKey)
	msg := tgbotapi.NewMessage(chatID, message)
	bot.Send(msg)
}

func fetch(url string) string {
	var text string

	resp, err := http.Get(url)
	if err != nil {
		return ""
	}
	defer resp.Body.Close()

	doc, err := goquery.NewDocumentFromReader(resp.Body)
	if err != nil {
		return ""
	}

	doc.Find("ul.og-grid").Each(func(i int, s *goquery.Selection) {
		text = strings.TrimSpace(s.Text())
	})

	return text
}
