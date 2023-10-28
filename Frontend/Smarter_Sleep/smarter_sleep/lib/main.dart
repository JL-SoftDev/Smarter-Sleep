import 'package:flutter/material.dart';
import 'package:smarter_sleep/app/appFrame.dart';
import 'app/screens/testingScreen.dart';
import 'package:amplify_flutter/amplify_flutter.dart';
import 'package:amplify_auth_cognito/amplify_auth_cognito.dart';
import 'package:amplify_authenticator/amplify_authenticator.dart';

import 'amplifyconfiguration.dart';

void main() {
  runApp(const SmarterSleep());
}

class SmarterSleep extends StatefulWidget {
  const SmarterSleep({super.key});

  @override
  State<SmarterSleep> createState() => _SmarterSleepState();
}

class _SmarterSleepState extends State<SmarterSleep> {
  @override
  void initState() {
    super.initState();
    _configureAmplify();
  }

  Future<void> _configureAmplify() async {
    try {
      final auth = AmplifyAuthCognito();
      await Amplify.addPlugin(auth);
      await Amplify.configure(amplifyconfig);
    } on Exception catch (e) {
      safePrint('An error occurred configuring Amplify: $e');
    }
  }

  @override
  Widget build(BuildContext context) {
    return Authenticator(
      child:
          MaterialApp(builder: Authenticator.builder(), home: const AppFrame()),
    );
  }
}
