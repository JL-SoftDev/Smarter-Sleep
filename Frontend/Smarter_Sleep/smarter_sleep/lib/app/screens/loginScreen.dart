import 'package:flutter/material.dart';
import 'package:smarter_sleep/app/widgets/loginInputWidget.dart';

import '../appFrame.dart';
import 'homeScreen.dart';

class LoginScreen extends StatefulWidget {
  const LoginScreen({super.key});

  @override
  State<LoginScreen> createState() => _LoginScreenState();
}

class _LoginScreenState extends State<LoginScreen> {
  bool isLoggingIn = false;
  TextEditingController _usernameController = TextEditingController();
  late TextEditingController _passwordController;

  @override
  void initState() {
    // TODO: implement initState
    super.initState();

    _passwordController = TextEditingController();
  }

  @override
  void dispose() {
    // TODO: implement dispose
    super.dispose();
    _usernameController.dispose();
    _passwordController.dispose();
  }

  @override
  Widget build(BuildContext context) {
    var exampleAvatarURL =
        "https://media.istockphoto.com/id/1300845620/vector/user-icon-flat-isolated-on-white-background-user-symbol-vector-illustration.jpg?s=612x612&w=0&k=20&c=yBeyba0hUkh14_jgv1OKqIH0CCSWU_4ckRkAoy2p73o=";

    return Scaffold(
        body: Stack(children: [
      //Login Screen Content
      Column(mainAxisAlignment: MainAxisAlignment.center, children: [
        //Text label
        Center(child: Text("Login Screen")),
        //Login Container
        Container(
            padding: EdgeInsets.symmetric(horizontal: 12, vertical: 4),
            width: MediaQuery.of(context).size.width * 0.80,
            height: MediaQuery.of(context).size.height * 0.65,
            //width: 200,
            //height: 250,
            color: Colors.grey.shade300,
            child: Column(
                crossAxisAlignment: CrossAxisAlignment.center,
                mainAxisAlignment: MainAxisAlignment.start,
                children: [
                  //Login Avatar Image
                  CircleAvatar(
                      radius: 48,
                      backgroundImage: Image.network(exampleAvatarURL).image),
                  //Spacer
                  SizedBox(height: 32),
                  Flexible(
                      child: Center(
                          child: Column(children: [
                    //Username Input Widget
                    LoginInputWidget(
                        controller: _usernameController,
                        label: "Username",
                        hintText: "Username, email, phonenumber"),
                    //Spacing
                    SizedBox(height: 32),
                    //Passsword Input Widget
                    LoginInputWidget(
                        controller: _passwordController,
                        obscureText: true,
                        label: "Password",
                        hintText: "Password"),
                    //Spacing
                    SizedBox(height: 48),
                    //Login Button
                    TextButton(onPressed: _loginAction, child: Text("Login")),
                  ])))
                ]))
      ]),

      //Progress Indicator with conditional operator
      isLoggingIn
          ? Container(
              color: Colors.black87,
              child: Center(child: CircularProgressIndicator()))
          : Container(),
    ]));
  }

  void _loginAction() async {
    print("TODO - Log in user using credentials");
    print("Username: ${_usernameController.text}");
    print("Password: ${_passwordController.text}");

    setState(() {
      isLoggingIn = true;
    });

    //Simulate network delay
    await Future.delayed(Duration(seconds: 2));

    //Navigate to our home screen
    mainNavigatorKey.currentState!.pushNamed("/home");
  }
}
