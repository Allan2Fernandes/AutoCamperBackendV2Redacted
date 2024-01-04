pipeline {
    agent any

    stages {
        stage('Start up') {
			steps {
				echo 'Starting up'
			}
		}
		
		stage('Build') {
            steps {
                echo 'Building'
            }
        }
		
		stage('Test') {
            steps {
               echo 'Testing'
			   //sh 'dotnet test'
			   dir('XUnitTestProject'){
					//Unit tests
					sh 'rm -rf coverage.cobertura.xml' // Clean up
					sh 'dotnet add package coverlet.collector' //needed to output a cobertura coverage report
					sh 'dotnet add package coverlet.msbuild'
					sh "dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:ExcludeByFile='**/*Migrations/*.cs'"

					archiveArtifacts 'coverage.cobertura.xml'

					publishCoverage adapters: [istanbulCoberturaAdapter(path: 'coverage.cobertura.xml', thresholds: [
						[failUnhealthy: true, thresholdTarget: 'Conditional', unhealthyThreshold: 6.0, unstableThreshold: 5.0]
					])], checksName: '', sourceFileResolver: sourceFiles('NEVER_STORE')

					//sh 'k6 run LoadTests/LoadTest.js'
					//sh 'k6 run LoadTests/SoakTest.js'
					//sh 'k6 run LoadTests/SpikeTest.js'
					//sh 'k6 run LoadTests/StressTest.js'
				}
			}
        }				
		
		stage('Deploy') {
			steps{
				echo 'Deploying ...'
				sh 'docker compose down'
				sh 'docker build . -t autocamperbackend'
				sh 'docker compose up -d'			
			}
		}
	}
 }