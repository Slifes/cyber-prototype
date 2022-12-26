from rest_framework.generics import ListAPIView, CreateAPIView
from rest_framework import permissions

from .models import Token, Authentication, SessionMap
from .serializers import CharacterSerializer, AuthenticationSerializer, SessionMapSerializer


class AuthenticateView(CreateAPIView):
    model = Authentication
    serializer_class = AuthenticationSerializer

authenticate_view = AuthenticateView.as_view()


class CharacterListView(ListAPIView):
    model = Token
    serializer_class = CharacterSerializer

    def get_queryset(self):
        return Token.objects.all()

character_list = CharacterListView.as_view()


class SessionMapCreateView(CreateAPIView):
    model = SessionMap
    serializer_class = SessionMapSerializer

session_create = SessionMapCreateView.as_view()
