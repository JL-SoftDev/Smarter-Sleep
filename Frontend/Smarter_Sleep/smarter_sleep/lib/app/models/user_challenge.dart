class UserChallenge {
  final int id;
  final int challengeId;
  final String challengeName;
  final String? challengeDesc;
  final DateTime? startDate;
  final DateTime? expireDate;
  final bool userSelected;
  final double completionPercentage;
  final int numCompleted;
  final int numTargetted;

  UserChallenge({
    required this.id,
    required this.challengeId,
    required this.challengeName,
    this.challengeDesc,
    this.startDate,
    this.expireDate,
    required this.userSelected,
    required this.completionPercentage,
    required this.numCompleted,
    required this.numTargetted,
  });
  factory UserChallenge.fromJson(Map<String, dynamic> json) {
    return UserChallenge(
      id: json['challengeLogId'],
      challengeId: json['challengeId'],
      challengeName: json['challengeName'],
      challengeDesc: json['challengeDescription'],
      startDate: DateTime.parse(json['startDate']),
      expireDate: DateTime.parse(json['expireDate']),
      userSelected: json['userSelected'],
      completionPercentage: json['completionPercentage'],
      numCompleted: json['completed'],
      numTargetted: json['goal'],
    );
  }
}
