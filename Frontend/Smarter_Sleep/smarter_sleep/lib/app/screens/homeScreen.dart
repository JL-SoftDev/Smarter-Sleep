import 'package:flutter/material.dart';

class HomeScreen extends StatefulWidget {
  const HomeScreen({super.key});

  @override
  State<HomeScreen> createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
            backgroundColor: Color.fromARGB(255, 36, 22, 65),
            leading: const Icon(Icons.circle, color: Colors.amber),
            leadingWidth: 40,
            actions: [const Icon(Icons.settings)]),
            bottomNavigationBar: BottomNavigationBar(
            backgroundColor: Color.fromARGB(255, 39, 22, 74),
            items: const [
              BottomNavigationBarItem(icon: Icon(Icons.square), label: 'Home'),
              BottomNavigationBarItem(icon: Icon(Icons.square), label: 'Shop'),
              BottomNavigationBarItem(icon: Icon(Icons.square), label: 'Stats')
            ],
            selectedItemColor: Colors.amber,
            unselectedItemColor: Colors.white,
            ),     
            body: ButtonGroup(),
    );
  }
}
class ButtonGroup extends StatelessWidget {
  const ButtonGroup({super.key});

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.all(25),
      child: Row(
        children: [
          OutlinedButton(onPressed:(){print('Alarms');}, 
                         child: Text('Alarms & Devices', style: TextStyle(color: Colors.white),),
                         style: ButtonStyle(backgroundColor: MaterialStatePropertyAll<Color>(Color.fromARGB(255, 134, 23, 159))),                     
            ),
          Spacer(),
          OutlinedButton(onPressed: (){print('Sleep');},
                         child: Text('Sleep', style: TextStyle(color: Colors.white)),
                  
                         style: ButtonStyle(backgroundColor: MaterialStatePropertyAll<Color>(Color.fromARGB(255, 134, 23, 159)),
                         shape: MaterialStatePropertyAll<CircleBorder>(CircleBorder()                                         
                         ),
                         fixedSize: MaterialStatePropertyAll<Size>(Size(90, 90)),
                         )
                         ),
        ],
      ),
    );
  }
}
