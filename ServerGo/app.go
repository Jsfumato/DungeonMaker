package main

import (
	"fmt"
	"os"

	"github.com/yuin/gopher-lua"
)

func main() {

	err := loadConfig("Config.lua")
	if err != nil {
		fmt.Println("[ERROR] cannot load config file")
		os.Exit(1)
	}

	server := NewOrbServer()
	err = server.Listen()

	if err != nil {
		fmt.Println("Could not serve")
		os.Exit(1)
	}

}

func loadConfig(fileName string) error {
	L := lua.NewState()
	defer L.Close()

	err := L.DoFile("fileName")
	if err != nil {
		panic(err)
		return err
	}

	return nil
}
