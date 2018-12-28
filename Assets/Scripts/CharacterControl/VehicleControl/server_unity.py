
import zmq
import time
import random

def tlist2tstring(torque_list):
        ret = ''
        for i in torque_list:
                ret+=str(i)
                ret+=','
        return ret[:-1]

def send_string(torque_string):
        context = zmq.Context()
        socket = context.socket(zmq.XPUB)
        socket.bind("tcp://*:12345")
        socket.recv()
        socket.send_string(torque_string)
        print(torque_string,'sent')
def send(torque_list):
        send_string(tlist2tstring(torque_list))
def send_rotation_reset():
        send_string('r,0,0,0')
def send_position_reset():
        send_string('p,20.9,0.8,20.9')
def send_reset():
        send_rotation_reset()
        send_position_reset()
if __name__=="__main__":
        send_string(tlist2tstring([1,1,1,1,1,1]))
        
 
        
