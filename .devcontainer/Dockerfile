FROM mcr.microsoft.com/dotnet/core/sdk:3.1

ARG USER_NAME="dev"
# change user password
ARG USER_PASSWORD="0000"

# https://gist.github.com/MichalZalecki/4a87880bbe7a3a5428b5aebebaa5cd97#gistcomment-2833222
ENV USER_NAME $USER_NAME
ENV USER_PASSWORD $USER_PASSWORD
ENV CONTAINER_IMAGE_VER=v1.0.0

RUN echo $USER_NAME
RUN echo $USER_PASSWORD
RUN echo $CONTAINER_IMAGE_VER

RUN apt update && \
    apt install -y sudo \
    curl \
    git-core \
    gnupg \
    locales \
    zsh \
    wget \
    nano \
    fonts-powerline \
    # set up locale
    && locale-gen en_US.UTF-8 \
    # add a user (--disabled-password: the user won't be able to use the account until the password is set)
    && adduser --quiet --disabled-password --shell /bin/zsh --home /home/$USER_NAME --gecos "User" $USER_NAME \
    # update the password
    && echo "${USER_NAME}:${USER_PASSWORD}" | chpasswd && usermod -aG sudo $USER_NAME

USER $USER_NAME

# run the installation script  
RUN wget https://github.com/robbyrussell/oh-my-zsh/raw/master/tools/install.sh -O - | zsh || true