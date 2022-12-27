from rest_framework.generics import ListAPIView, CreateAPIView

from .models import Token, Authentication, SessionMap
from .serializers import CharacterSerializer, AuthenticationSerializer, SessionMapSerializer
from .permissions import IsServerAuthenticated


class AuthenticateView(CreateAPIView):
    model = Authentication
    serializer_class = AuthenticationSerializer

authenticate_view = AuthenticateView.as_view()


class CharacterListView(ListAPIView):
    model = Token
    serializer_class = CharacterSerializer
    permission_classes = (IsServerAuthenticated, )

    def get_queryset(self):
        return Token.objects\
            .filter(owner=self.request.user_server.address)

character_list = CharacterListView.as_view()


class SessionMapCreateView(CreateAPIView):
    model = SessionMap
    serializer_class = SessionMapSerializer
    permission_classes = (IsServerAuthenticated, )

session_create = SessionMapCreateView.as_view()
