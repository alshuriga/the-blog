pipeline {
    agent any

    stages {
        stage('Clean workspace') {
            steps {
                cleanWs()
            }
        }
        stage('Pull repo')
        {
            steps {
                git branch: 'main', credentialsId: '131f0c00-a3c3-4f8a-8316-a816e1a835cd', url: 'git@github.com:alshuriga/mini-blog.git'
            }
        }
        stage('Run Tests') {
            steps {
                bat ' dotnet test tests/MiniBlog.Tests.csproj'
            }
        }
    }
}
