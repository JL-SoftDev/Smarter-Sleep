import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;

class TestDeviceConnection extends StatefulWidget{
  const TestDeviceConnection({super.key});
  
  @override
  State<StatefulWidget> createState() => testingDeviceConnectionState();
}

class Album {
  final int userId;
  final int id;
  final String title;

  const Album({
    required this.userId,
    required this.id,
    required this.title,
  });

  factory Album.fromJson(Map<String, dynamic> json) {
    return Album(
      userId: json['userId'],
      id: json['id'],
      title: json['title'],
    );
  }
}
class testingDeviceConnectionState extends State<TestDeviceConnection>{
  late Future<Album> futureAlbum;

  @override
  void initState() {
    super.initState();
    futureAlbum = fetchAlbum();
  }

  @override
  Widget build(BuildContext context) {
    double sizeHeight = MediaQuery.of(context).size.height;
    double sizeWidth = MediaQuery.of(context).size.width;
    return Scaffold(
      body:Column(
        children: [
        Container(
          height: sizeHeight,
          width: sizeWidth,
          child: ListView(
          scrollDirection: Axis.horizontal,
          children: [
            SizedBox(width: 100),
            IconButton(onPressed: _addLightDevice, icon:Icon(Icons.lightbulb), iconSize: 50,),
            const SizedBox(width: 20),
            IconButton(onPressed: _addTemperatureDevice, icon:Icon(Icons.thermostat), iconSize: 50),
            const SizedBox(width: 20),
            IconButton(onPressed: _addSoundDevice, icon:Icon(Icons.speaker), iconSize: 50),
            const SizedBox(width: 20),
          ],
          ),
        )
      ],)
    );

    throw UnimplementedError();
  }
void _addTemperatureDevice(){

}

void _addSoundDevice(){

}

void _addLightDevice(){

}
Future<Album> fetchAlbum() async {
  final response = await http
      .get(Uri.parse('http://ec2-54-87-139-255.compute-1.amazonaws.com/'));

  if (response.statusCode == 200) {
    // If the server did return a 200 OK response,
    // then parse the JSON.
    return Album.fromJson(jsonDecode(response.body));
  } else {
    // If the server did not return a 200 OK response,
    // then throw an exception.
    throw Exception('Failed to load album');
  }
}
}