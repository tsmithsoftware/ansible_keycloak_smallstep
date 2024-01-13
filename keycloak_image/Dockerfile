FROM registry.access.redhat.com/ubi9 AS ubi-micro-build
#COPY mycertificate.crt /etc/pki/ca-trust/source/anchors/mycertificate.crt
#COPY openssl.cnf /etc/pki/ca-trust/source/anchors/mycertificate.crt
# install python to allow administration via Ansible
RUN mkdir -p /mnt/rootfs
RUN dnf install --installroot /mnt/rootfs python3 --releasever 9 --setopt install_weak_deps=false --nodocs -y && \
    dnf --installroot /mnt/rootfs clean all && \
    rpm --root /mnt/rootfs -e --nodeps setup
FROM quay.io/keycloak/keycloak
COPY --from=ubi-micro-build /etc/pki /etc/pki
COPY --from=ubi-micro-build /mnt/rootfs /

ENTRYPOINT []
