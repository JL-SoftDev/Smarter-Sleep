class UserChallenge {
  final String challengeName;
  final DateTime startDate;
  final DateTime expireDate;
  final bool userSelected;
  final double completionPercentage;

  UserChallenge({
    required this.challengeName,
    required this.startDate,
    required this.expireDate,
    required this.userSelected,
    required this.completionPercentage,
  });
  factory UserChallenge.fromJson(Map<String, dynamic> json) {
    return UserChallenge(
      challengeName: "Default",
      startDate: DateTime.parse(json['startDate']),
      expireDate: DateTime.parse(json['expireDate']),
      userSelected: json['userSelected'],
      completionPercentage: json['completionPercentage'],
    );
  }
}
