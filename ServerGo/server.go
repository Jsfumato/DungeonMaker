package main

import (
	"fmt"
	"net"
)

type MainServer struct {
	host  net.IP
	port  uint16
	world *World
}

func NewMainServer() *MainServer {
	return &MainServer{
		host:  net.IPv4(192, 168, 1, 75),
		port:  9090,
		world: NewWorld(),
	}
}

func (self *MainServer) BasePath() string {
	return fmt.Sprintf("%s:%d", self.host.String(), self.port)
}

func (self *MainServer) Listen() error {

	server, err := net.Listen("tcp", self.BasePath())
	defer server.Close()

	if err != nil {
		return err
	}

	self.world.Start()

	for {
		conn, err := server.Accept()
		if err != nil {
			fmt.Println("Error accepting incoming connection: ", err)
		} else {
			self.world.Register(conn)
		}

	}
}
