#! /usr/local/bin/python3
import zmq
import time
import random


def GetString():
    context = zmq.Context()
    socket = context.socket(zmq.REQ)
    socket.connect("tcp://localhost:12346")

    TIMEOUT = 10000
    socket.send_string("request")
    poller = zmq.Poller()
    poller.register(socket, zmq.POLLIN)
    evt = dict(poller.poll(TIMEOUT))
    if evt:
        if evt.get(socket) == zmq.POLLIN:
            response = socket.recv(zmq.NOBLOCK)
            print(response)
            socket.close()
            return response
    time.sleep(1)
    
def ProcessString(input_string):
        input_string = input_string.decode("utf-8")
        euler_angles = input_string.split(',')
        ret = []
        for i in euler_angles:
                ret.append(float(i))
                
        return ret
def GetEuler():
        return ProcessString(GetString())
if __name__ == "__main__":
        
        print(GetEuler())
