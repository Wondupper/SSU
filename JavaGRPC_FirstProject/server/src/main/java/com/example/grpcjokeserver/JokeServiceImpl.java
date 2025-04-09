package com.example.grpcjokeserver;

import com.example.grpcjokeserver.grpc.JokeServiceGrpc;
import com.example.grpcjokeserver.grpc.JokeServiceProto.AgeRequest;
import com.example.grpcjokeserver.grpc.JokeServiceProto.Empty;
import com.example.grpcjokeserver.grpc.JokeServiceProto.GenreRequest;
import com.example.grpcjokeserver.grpc.JokeServiceProto.JokeResponse;
import io.grpc.stub.StreamObserver;
import net.devh.boot.grpc.server.service.GrpcService;
import org.springframework.beans.factory.annotation.Autowired;

@GrpcService
public class JokeServiceImpl extends JokeServiceGrpc.JokeServiceImplBase {

    @Autowired
    private JokeApiClient jokeApiClient;

    @Override
    public void getRandomJoke(Empty request, StreamObserver<JokeResponse> responseObserver) {
        String joke = jokeApiClient.getRandomJoke();
        JokeResponse response = JokeResponse.newBuilder()
                .setJoke(joke)
                .setError(false)
                .build();
        responseObserver.onNext(response);
        responseObserver.onCompleted();
    }

    @Override
    public void getJokeByGenre(GenreRequest request, StreamObserver<JokeResponse> responseObserver) {
        String genre = request.getGenre();
        String joke = jokeApiClient.getJokeByGenre(genre);
        JokeResponse response = JokeResponse.newBuilder()
                .setJoke(joke)
                .setError(false)
                .build();
        responseObserver.onNext(response);
        responseObserver.onCompleted();
    }

    @Override
    public void getJokeByAge(AgeRequest request, StreamObserver<JokeResponse> responseObserver) {
        int age = request.getAge();
        String joke = jokeApiClient.getJokeByAge(age);
        JokeResponse response = JokeResponse.newBuilder()
                .setJoke(joke)
                .setError(false)
                .build();
        responseObserver.onNext(response);
        responseObserver.onCompleted();
    }
}