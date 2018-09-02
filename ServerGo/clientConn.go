package main

import (
	"errors"
	"io"
	"net"
	"sync"
	"time"
)

//
// A mock implementation of net.Conn. This will be used across multiple goroutines
// in the unit tests so I've tried to make it as bullet proof as possible using a
// read / write lock.
//
type ClientConn struct {
	lock sync.RWMutex

	clientReadData  []byte
	clientReadError error

	clientWriteData  []byte
	clientWriteError error

	closed    bool
	closedErr error

	written chan bool
}

// @note - Does not copy the errors so assumed that they are either immutable or are
// thread safe.

func (self *ClientConn) SetClientWriteError(err error) {

	self.lock.Lock()
	defer self.lock.Unlock()

	self.clientWriteError = err
}

func (self *ClientConn) SetClientReadError(err error) {

	self.lock.Lock()
	defer self.lock.Unlock()

	self.clientReadError = err
}

func (self *ClientConn) ReadData(readBytes []byte) {

	self.lock.Lock()
	defer self.lock.Unlock()

	self.clientReadData = make([]byte, len(readBytes))
	copy(self.clientReadData, readBytes)
}

func (self *ClientConn) WriteData() []byte {

	self.lock.RLock()
	defer self.lock.RUnlock()

	writeBytes := make([]byte, len(self.clientWriteData))
	copy(writeBytes, self.clientWriteData)

	return writeBytes
}

func (self *ClientConn) Read(b []byte) (n int, err error) {

	self.lock.Lock()
	defer self.lock.Unlock()

	if self.clientReadError != nil {
		err = self.clientReadError
	} else {
		n = len(self.clientReadData)
		copy(b, self.clientReadData)
	}
	self.clientReadError = io.EOF

	return
}

func (self *ClientConn) Write(b []byte) (n int, err error) {

	self.lock.Lock()
	defer self.lock.Unlock()

	if self.clientWriteError != nil {
		err = self.clientWriteError
	} else {
		n = len(b)
		self.clientWriteData = make([]byte, n)
		copy(self.clientWriteData, b)
		self.broadcastWrite(n)
	}

	return
}

func (self *ClientConn) broadcastWrite(n int) {

	// @note - The problem with this is that it _assumes_ we're running not on the
	// recieving thread, thus if Write is executed on the same thread that recieves
	// from this chan we get deadlock.

	if self.written != nil && n > 0 {
		self.written <- true
	}
}

func (self *ClientConn) Close() error {

	self.lock.Lock()
	defer self.lock.Unlock()

	self.closed = true
	return self.closedErr
}

func (self *ClientConn) Closed() bool {

	self.lock.RLock()
	defer self.lock.RUnlock()

	return self.closed
}

func (self *ClientConn) LocalAddr() net.Addr                { return nil }
func (self *ClientConn) RemoteAddr() net.Addr               { return nil }
func (self *ClientConn) SetDeadline(t time.Time) error      { return nil }
func (self *ClientConn) SetReadDeadline(t time.Time) error  { return nil }
func (self *ClientConn) SetWriteDeadline(t time.Time) error { return nil }

func NewClientConn(written chan bool) *ClientConn {
	return &ClientConn{
		clientWriteData: make([]byte, 2048),
		closedErr:       errors.New("Error on close"),
		written:         written,
	}
}
