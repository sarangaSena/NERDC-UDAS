/*
 Name:		SCADA_Final_Code.ino
 Created:	4/19/2018 9:45:37 AM
 Author:	Mechatronic S.M.S Saranga Senarathna
*/


#include <MCP3424.h>
#include <Wire.h>
#include <SPI.h>
#include <Ethernet.h>

#include "Mudbus.h"



MCP3424 MCP(0); // Declaration of MCP3424 pin addr1 et addr0 are connected to +5V
Mudbus Mb;

long Voltage[4]; // Array used to store results

void setup() {

	uint8_t mac[] = { 0x90, 0xA2, 0xDA, 0x00, 0x51, 0x06 };
	uint8_t ip[] = { 172, 16, 11, 15 };
	uint8_t gateway[] = { 172, 16, 1, 254 };
	uint8_t subnet[] = { 255, 255, 0, 0 };
	Ethernet.begin(mac, ip, gateway, subnet);
	//Serial.begin(9600);  // start serial for output
	MCP.begin(0);

	pinMode(23, OUTPUT);
	pinMode(25, OUTPUT);
	pinMode(27, OUTPUT);
	pinMode(29, OUTPUT);

	pinMode(31, OUTPUT);
	pinMode(33, OUTPUT);
	pinMode(35, OUTPUT);
	pinMode(37, OUTPUT);

	pinMode(22, INPUT_PULLUP);
	pinMode(24, INPUT_PULLUP);
	pinMode(26, INPUT_PULLUP);
	pinMode(28, INPUT_PULLUP);

	pinMode(30, INPUT_PULLUP);
	pinMode(32, INPUT_PULLUP);
	pinMode(34, INPUT_PULLUP);
	pinMode(36, INPUT_PULLUP);

	digitalWrite(23, HIGH);
	digitalWrite(25, HIGH);
	digitalWrite(27, HIGH);
	digitalWrite(29, HIGH);


	digitalWrite(31, HIGH);
	digitalWrite(33, HIGH);
	digitalWrite(35, HIGH);
	digitalWrite(37, HIGH);

}


void loop() {

	Mb.Run();


	//Serial.println("");
	//Serial.println("16 bits");

	digitalWrite(23, !Mb.C[0]);
	digitalWrite(25, !Mb.C[1]);
	digitalWrite(27, !Mb.C[2]);
	digitalWrite(29, !Mb.C[3]);
	delayMicroseconds(100);

	digitalWrite(31, !Mb.C[4]);
	digitalWrite(33, !Mb.C[5]);
	digitalWrite(35, !Mb.C[6]);
	digitalWrite(37, !Mb.C[7]);
	delayMicroseconds(100);


	Mb.I[7] = !digitalRead(22);
	Mb.I[6] = !digitalRead(24);
	Mb.I[5] = !digitalRead(26);
	Mb.I[4] = !digitalRead(28);
	delayMicroseconds(100);

	Mb.I[3] = !digitalRead(30);
	Mb.I[2] = !digitalRead(32);
	Mb.I[1] = !digitalRead(34);
	Mb.I[0] = !digitalRead(36);
	delayMicroseconds(100);


	MCP.configuration(1, 16, 1, 1); 

	Voltage[0] = MCP.measure(); 
	int out1 = Voltage[0] / 62.5;
	Mb.IR[0] = out1;
	delayMicroseconds(100);

	MCP.configuration(2, 16, 1, 1);  

	Voltage[1] = MCP.measure(); 
	int out2 = Voltage[1] / 62.5;
	Mb.IR[1] = out2;
	delayMicroseconds(100);

	MCP.configuration(3, 16, 1, 1); 

	Voltage[2] = MCP.measure(); 
	int out3 = Voltage[2] / 62.5;
	Mb.IR[2] = out3;
	delayMicroseconds(100);

	MCP.configuration(4, 16, 1, 1); 

	Voltage[3] = MCP.measure(); 
	int out4 = Voltage[3] / 62.5;
	Mb.IR[3] = out4;
	delayMicroseconds(100);

}
